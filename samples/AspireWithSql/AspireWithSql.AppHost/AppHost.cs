var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureAppServiceEnvironment("aspirewithsql");

// Create SQL Server & Database
var sqlServer = builder.AddAzureSqlServer("sqlsrv");
var sqlDb = sqlServer.AddDatabase("aspiredb")
    .WithDefaultAzureSku();

var apiService = builder.AddProject<Projects.AspireWithSql_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(sqlDb)
    .WaitFor(sqlDb);

builder.AddProject<Projects.AspireWithSql_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
