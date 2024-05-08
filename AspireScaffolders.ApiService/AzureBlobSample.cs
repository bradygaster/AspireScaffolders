using Azure.Storage.Blobs;
using System.Text;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder;

public class Entry
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public static class AzureBlobStorageSampleExtensions
{
    public static WebApplication MapBlobStorageSampleApi(this WebApplication app)
    {
        app.MapGet("/blobstoragesampleapi", async (BlobServiceClient blobServiceClient) =>
        {
            var entry = new Entry { Id = Guid.NewGuid() };

            var blobContainerClient = blobServiceClient.GetBlobContainerClient("entries");
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient($"{entry.Id.ToString()}.json");
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(entry))))
            {
                await blobClient.UploadAsync(ms);
            }

            var blobs = blobContainerClient.GetBlobs();
            return blobs;
        });

        return app;
    }
}