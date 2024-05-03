using AwesomeBlazor.Models;
using Azure;
using Azure.Data.Tables;

namespace AwesomeBlazor.Store;

public class AwesomeBlazorStore(
    TableServiceClient tableServiceClient,
    HttpClient httpClient
)
{
    private const string _awesomeBlazorUrl = "https://raw.githubusercontent.com/AdrienTorris/awesome-blazor/master/README.md";

    private AwesomeResourceGroup? _rootGroup;

    private readonly SemaphoreSlim _syncer = new(1);

    public async ValueTask<AwesomeResourceGroup> GetAwesomeBlazorContentAsync()
    {
        await this._syncer.WaitAsync();
        try
        {
            if (this._rootGroup is null)
            {
                if ((this._rootGroup = await this.TryLoadFromTableStorageAsync()) == null)
                {
                    var awesomeBlazorContents = await httpClient.GetStringAsync(_awesomeBlazorUrl);
                    this._rootGroup = AwesomeBlazorParser.ParseMarkdown(awesomeBlazorContents);
                    await this.SaveToTableStorageAsync(this._rootGroup);
                }
            }
        }
        finally { this._syncer.Release(); }

        return this._rootGroup;
    }

    internal async ValueTask<AwesomeResourceGroup?> TryLoadFromTableStorageAsync()
    {
        throw new NotImplementedException();
    }

    private class TableClients(TableServiceClient tableServiceClient)
    {
        internal TableClient Groups { get; } = tableServiceClient.GetTableClient("groups");
        internal TableClient Resources { get; } = tableServiceClient.GetTableClient("resources");
    }

    internal class Entry : ITableEntity
    {
        public required string PartitionKey { get; set; }

        public required string RowKey { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public bool Indexed { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public byte[]? Buff { get; set; }
    }


    internal async ValueTask SaveToTableStorageAsync(AwesomeResourceGroup rootGroup)
    {
        await tableServiceClient.CreateTableIfNotExistsAsync("groups");
        await tableServiceClient.CreateTableIfNotExistsAsync("resources");

        var tableClients = new TableClients(tableServiceClient);
        await this.SaveToTableStorageAsync(tableClients, rootGroup);
    }

    private async ValueTask SaveToTableStorageAsync(TableClients tableClients, AwesomeResourceGroup parentGroup)
    {
        foreach (var group in parentGroup.SubGroups)
        {
            var groupEntity = AwesomeResourceGroupEntity.CreateFrom(group);
            await tableClients.Groups.AddEntityAsync(groupEntity);
            await this.SaveToTableStorageAsync(tableClients, group);
        }
    }

}
