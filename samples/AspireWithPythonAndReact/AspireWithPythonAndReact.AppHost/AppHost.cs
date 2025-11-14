var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureAppServiceEnvironment("aspirewithpython");

var apiService = builder.AddPythonApp("apiservice", "../PythonApiService", "main.py")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

var apiEndpoint = apiService.GetEndpoint("http");

var webFrontend = builder.AddNpmApp("webfrontend", "../NodeWebFrontend")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("APISERVICE_BASE_URL", apiEndpoint)
    .PublishAsDockerFile();

apiService
    .WithEnvironment("ALLOWED_ORIGINS", webFrontend.Resource.GetEndpoint("http"));

builder.Build().Run();
