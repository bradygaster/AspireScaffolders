var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var blobs = storage.AddBlobs("blobs");

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice")
                        .WithReference(blobs);

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
