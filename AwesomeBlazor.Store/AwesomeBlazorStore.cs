using AwesomeBlazor.Models;
using AwesomeBlazor.Store.Extensions;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using SmartComponents.LocalEmbeddings;

namespace AwesomeBlazor.Store;

public class AwesomeBlazorStore(
    IServiceProvider services,
    TableServiceClient tableServiceClient,
    HttpClient httpClient
) : IAsyncDisposable
{
    private const string _awesomeBlazorUrl = "https://raw.githubusercontent.com/AdrienTorris/awesome-blazor/master/README.md";

    private readonly SemaphoreSlim _syncer = new(1);

    private IEnumerable<AwesomeResourceGroupEntity>? _groupEntities;

    private IEnumerable<AwesomeResourceEntity>? _resourceEntities;

    private readonly Lazy<LocalEmbedder> _embedder = new(() => services.GetRequiredService<LocalEmbedder>());

    private IReadOnlyDictionary<string, EmbeddingF32>? _embeddings;

    private Task? _savingTask;

    private readonly CancellationTokenSource _canceller = new();

    public async ValueTask<AwesomeResourceGroup> GetAwesomeBlazorContentAsync()
    {
        await this._syncer.WaitAsync();
        try
        {
            var rootGroup = await this.TryLoadFromTableStorageAsync();
            if (rootGroup == null)
            {
                var awesomeBlazorContents = await httpClient.GetStringAsync(_awesomeBlazorUrl);
                rootGroup = AwesomeBlazorParser.ParseMarkdown(awesomeBlazorContents);
                this._savingTask = this.SaveToTableStorageAsync(rootGroup, this._canceller.Token);

                //this._embeddings = await this.UpdateEmbeddingsAsync(rootGroup);
            }
            return rootGroup;
        }
        finally { this._syncer.Release(); }
    }

    private class TableClients(TableServiceClient tableServiceClient)
    {
        internal TableClient Groups { get; } = tableServiceClient.GetTableClient("groups");
        internal TableClient Resources { get; } = tableServiceClient.GetTableClient("resources");
    }

    internal async ValueTask<AwesomeResourceGroup?> TryLoadFromTableStorageAsync()
    {
        // check if the "groups" and "resources" tables exist
        if (this._groupEntities == null || this._resourceEntities == null)
        {
            if (!await tableServiceClient.ExistTableAsync("groups")) return null;
            if (!await tableServiceClient.ExistTableAsync("resources")) return null;

            var tableClients = new TableClients(tableServiceClient);
            this._groupEntities = await tableClients.Groups.ToListAsync<AwesomeResourceGroupEntity>();
            this._resourceEntities = await tableClients.Resources.ToListAsync<AwesomeResourceEntity>();
        }

        var groups = new Dictionary<string, AwesomeResourceGroup>();
        foreach (var groupEntty in this._groupEntities)
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

        foreach (var resourceEntty in this._resourceEntities)
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

    internal async Task SaveToTableStorageAsync(AwesomeResourceGroup rootGroup, CancellationToken cancellationToken = default)
    {
        var groupEntities = new List<AwesomeResourceGroupEntity>();
        var resourceEntities = new List<AwesomeResourceEntity>();
        await rootGroup.ForEachAllAsync(
            (group) => { if (group.Id != "/") groupEntities.Add(AwesomeResourceGroupEntity.CreateFrom(group)); return ValueTask.CompletedTask; },
            (resource) => { resourceEntities.Add(AwesomeResourceEntity.CreateFrom(resource)); return ValueTask.CompletedTask; }
        );
        this._groupEntities = groupEntities;
        this._resourceEntities = resourceEntities;

        await tableServiceClient.CreateTableIfNotExistsAsync("groups");
        await tableServiceClient.CreateTableIfNotExistsAsync("resources");
        var tableClients = new TableClients(tableServiceClient);
        foreach (var groupEntity in groupEntities) await tableClients.Groups.AddEntityAsync(groupEntity, cancellationToken);
        foreach (var resourceEntity in resourceEntities) await tableClients.Resources.AddEntityAsync(resourceEntity, cancellationToken);
    }

    internal async ValueTask<IReadOnlyDictionary<string, EmbeddingF32>> UpdateEmbeddingsAsync(AwesomeResourceGroup rootGroup)
    {
        var embedder = this._embedder.Value;
        var embeddings = new Dictionary<string, EmbeddingF32>();

        ValueTask updater<T>(T item, Func<string> getTextExpression) where T : IEmbeddingSource
        {
            if (item.Id == "/") return ValueTask.CompletedTask;
            var embedding = item.Embedding?.Any() == true ? new EmbeddingF32(item.Embedding) : embedder.Embed(getTextExpression());
            if (item.Embedding?.Any() != true) item.Embedding = embedding.Buffer.ToArray();
            embeddings.Add(item.Id, embedding);
            return ValueTask.CompletedTask;
        }

        await rootGroup.ForEachAllAsync(
            g => updater(g, () => g.Title + "\n" + g.ParagraphsHtml),
            r => updater(r, () => r.Title + "\n" + r.DescriptionText));
        return embeddings;
    }

    public void UpdateVisibiltyBySemanticFilter(string searchText)
    {
        var searchEmbedding = this._embedder.Value.Embed(searchText);
    }

    public async ValueTask DisposeAsync()
    {
        this._canceller.Cancel();
        if (this._savingTask != null) try { await this._savingTask; } catch { }
        this._canceller.Dispose();
    }
}
