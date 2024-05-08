var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice");

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
