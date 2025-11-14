using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddAzureAppServiceEnvironment("aspirewithopenai");

var connectionString = builder.Configuration.GetConnectionString("gpt4o");

var openai =
    string.IsNullOrWhiteSpace(connectionString) ?
        builder.AddAzureOpenAI("openai")
        .AddDeployment("gpt4o", "gpt-4o", "2024-11-20")
        : builder.AddConnectionString("gpt4o");

var apiService = builder.AddProject<Projects.AspireWithOpenAI_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(openai);

builder.AddProject<Projects.AspireWithOpenAI_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
