using AspireWithSql.ApiService;
using AspireWithSql.ApiService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Configure DbContext with SQL Server provider.
builder.AddSqlServerDbContext<WeatherForecastContext>(connectionName: "aspiredb");

builder.Services.AddHostedService<DatabaseMigrations>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "API service is running. Navigate to /weatherforecast to see sample data.");

app.MapGet("/weatherforecast", async (WeatherForecastContext dbContext) =>
{
    var query = dbContext.Forecasts.OrderBy(f => f.Date).ToList();

    return query.Select(f => new WeatherForecast(f.Date, f.TemperatureC, f.Summary)).ToArray();
})
.WithName("GetWeatherForecast");

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
