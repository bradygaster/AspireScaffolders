var builder = DistributedApplication.CreateBuilder(args);

var sqldb = builder.AddSqlServer("sqlserver")
                 .AddDatabase("sqldb");

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice")
                        .WithReference(sqldb);

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
