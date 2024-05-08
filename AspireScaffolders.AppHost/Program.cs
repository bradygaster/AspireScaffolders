var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var entries = storage.AddTables("entries");

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice")
                        .WithReference(entries);

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
