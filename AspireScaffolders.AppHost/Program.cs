var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice");

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(redis);

builder.Build().Run();
