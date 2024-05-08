var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var queues = storage.AddQueues("queues");

var apiService = builder.AddProject<Projects.AspireScaffolders_ApiService>("apiservice")
                        .WithReference(queues);

builder.AddProject<Projects.AspireScaffolders_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(queues);

builder.Build().Run();
