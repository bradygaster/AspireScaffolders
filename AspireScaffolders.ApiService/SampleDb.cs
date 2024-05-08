using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Builder;

public class Entry
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

public class EntryDbContext(DbContextOptions<EntryDbContext> options) : DbContext(options)
{
    public DbSet<Entry> Entries { get; set; }
}

public static class EntrySampleDatabaseApiEndpointExtensions
{
    public static WebApplication MapSampleDatabaseApis(this WebApplication app)
    {
        app.MapGet("/", async (EntryDbContext entryDbContext) =>
        {
            // You wouldn't normally do this on every call,
            // but doing it here just to make this simple.

            await entryDbContext.Database.EnsureCreatedAsync();

            var entry = new Entry();
            await entryDbContext.Entries.AddAsync(entry);
            await entryDbContext.SaveChangesAsync();

            var entries = await entryDbContext.Entries.ToListAsync();

            return new
            {
                totalEntries = entries.Count,
                entries = entries
            };
        });

        return app;
    }
}