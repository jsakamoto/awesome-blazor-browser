using AwesomeBlazor.Models;
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

            foreach (var resource in group.Resources)
            {
                var resourceEntity = AwesomeResourceEntity.CreateFrom(resource);
                await tableClients.Resources.AddEntityAsync(resourceEntity);
            }

            await this.SaveToTableStorageAsync(tableClients, group);
        }
    }

}
