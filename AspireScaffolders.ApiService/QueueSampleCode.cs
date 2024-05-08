
using Azure.Storage.Queues;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder;

public class Entry
{
    public Guid Value { get; set; } = Guid.NewGuid();
}

public class QueueReadingWorker(QueueServiceClient queueServiceClient) : BackgroundService
{
    private readonly QueueServiceClient queueServiceClient = queueServiceClient;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var queue = queueServiceClient.GetQueueClient("incoming");
        await queue.CreateIfNotExistsAsync();
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var queue = queueServiceClient.GetQueueClient("incoming");
            var message = await queue.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
            if(message != null && message.Value != null)
            {
                await queue.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt, stoppingToken);
            }

            await Task.Delay(1000);
        }
    }
}

public static class AzureStorageQueueExampleExtensions
{
    public static WebApplicationBuilder AddAzureStorageQueueExample(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<QueueReadingWorker>();

        return builder;
    }

    public static WebApplication MapAzureStorageQueueExampleAPI(this WebApplication app)
    {
        app.MapGet("/azurestoragequeueexample", async (QueueServiceClient queueServiceClient) =>
        {
            var entry = new Entry();
            var queue = queueServiceClient.GetQueueClient("incoming");
            await queue.SendMessageAsync(JsonSerializer.Serialize(entry));
            return entry;
        });

        return app;
    }
}
