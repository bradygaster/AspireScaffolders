using Azure.Data.Tables;
using Azure;

namespace Microsoft.AspNetCore.Builder;

public class Entry : ITableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PartitionKey { get; set; } = "Sample";
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

public static class EntrySampleDatabaseApiEndpointExtensions
{
    public static WebApplication MapAzureTableSampleApi(this WebApplication app)
    {
        app.MapGet("/azuretablesample", async (TableServiceClient tableServiceClient) =>
        {
            var entry = new Entry();
            await tableServiceClient.CreateTableIfNotExistsAsync("entries");
            var entryTable = tableServiceClient.GetTableClient("entries");
            entry.RowKey = Guid.NewGuid().ToString();
            entryTable.AddEntity(entry);
            var entries = entryTable.QueryAsync<Entry>();
            return entries;
        });

        return app;
    }
}