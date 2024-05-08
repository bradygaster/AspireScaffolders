var builder = DistributedApplication.CreateBuilder(args);

var postgresqldb = builder.AddPostgres("postgresql").AddDatabase("postgresqldb");

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice")
                        .WithReference(postgresqldb);

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
