using AwesomeBlazor.Models;
using AwesomeBlazor.Store.Extensions;
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
    private class TableClients(TableServiceClient tableServiceClient)
    {
        internal TableClient Groups { get; } = tableServiceClient.GetTableClient("groups");
        internal TableClient Resources { get; } = tableServiceClient.GetTableClient("resources");
    }

    internal async ValueTask<AwesomeResourceGroup?> TryLoadFromTableStorageAsync()
    {
        // check if the "groups" and "resources" tables exist
        if (!await tableServiceClient.ExistTableAsync("groups")) return null;
        if (!await tableServiceClient.ExistTableAsync("resources")) return null;

        var tableClients = new TableClients(tableServiceClient);

        var groups = new Dictionary<string, AwesomeResourceGroup>();
        await foreach (var groupEntty in tableClients.Groups.QueryAsync<AwesomeResourceGroupEntity>())
        {
            var group = groupEntty.ConvertToResourceGroup();
            groups.Add(group.Id, group);
        }

        var rootGroup = new AwesomeResourceGroup();
        groups.Add(rootGroup.Id, rootGroup);
        foreach (var group in groups.Values)
        {
            if (group.Id == "/") continue;
            if (groups.TryGetValue(group.ParentId, out var parentGroup))
            {
                parentGroup.SubGroups.Add(group);
            }
        }

        await foreach (var resourceEntty in tableClients.Resources.QueryAsync<AwesomeResourceEntity>())
        {
            var resource = resourceEntty.ConvertToResource();
            if (groups.TryGetValue(resource.ParentId, out var parentGroup))
            {
                parentGroup.Resources.Add(resource);
            }
        }

        rootGroup.ReorderChildren();

        return rootGroup.SubGroups.Any() ? rootGroup : null;
    }


    internal async ValueTask SaveToTableStorageAsync(AwesomeResourceGroup rootGroup)
    {
        await tableServiceClient.CreateTableIfNotExistsAsync("groups");
        await tableServiceClient.CreateTableIfNotExistsAsync("resources");

        var tableClients = new TableClients(tableServiceClient);
        await rootGroup.ForEachAllAsync(
            async (group) => { if (group.Id != "/") await tableClients.Groups.AddEntityAsync(AwesomeResourceGroupEntity.CreateFrom(group)); },
            async (resource) => await tableClients.Resources.AddEntityAsync(AwesomeResourceEntity.CreateFrom(resource))
        );
    }
}
