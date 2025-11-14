using AspireWithOpenAI.ApiService;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddAzureOpenAIClient("gpt4o")
    .AddChatClient("gpt4o");

builder.Services.AddChatClient(services => services
    .GetRequiredService<AzureOpenAIClient>()
    .GetChatClient("gpt4o")
    .AsIChatClient()
    .AsBuilder()
    .Build());

builder.Services.AddTransient<WeatherSummaryService>();
builder.Services.AddTransient<ChatService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "API service is running. Navigate to /weatherforecast to see sample data.");

app.MapGet("/weatherforecast", async (WeatherSummaryService weatherSummaryService) =>
{
    List<WeatherForecast> forecasts = new();
    for (int i = 0; i < 5; i++)
    {
        var temperatureC = Random.Shared.Next(-20, 55);
        var message = new Message { 
            IsAssistant = false, 
            Content = $"Can you generate summary for temperature {temperatureC} C?" 
        };
        var summaryResponse = await weatherSummaryService.GetWeatherSummary(new ChatRequest(new List<Message> { message }));
        forecasts.Add(new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(i + 1)),
            temperatureC,
            summaryResponse.Content));
    }

    return forecasts;
})
.WithName("GetWeatherForecast");

app.MapPost("/chat", async (ChatRequest request, ChatService chatService) =>
{
    var response = await chatService.GetChatResponse(request);
    return Results.Ok(response);
})
.WithName("GetChatResponse");

app.MapGet("/health", () => Results.Ok("Healthy"))
    .WithName("HealthCheck");

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
