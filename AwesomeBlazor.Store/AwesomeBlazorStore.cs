using AwesomeBlazor.Models;
using AwesomeBlazor.Store.Extensions;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using SmartComponents.LocalEmbeddings;

namespace AwesomeBlazor.Store;

//using EmbeddingType = SmartComponents.LocalEmbeddings.EmbeddingF32;
using EmbeddingType = SmartComponents.LocalEmbeddings.EmbeddingI1;

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

    private Task? _savingTask;

    private Task<IReadOnlyDictionary<string, EmbeddingType>>? _embeddingsTask;

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
            }
            this._embeddingsTask = Task.Run(async () => await this.UpdateEmbeddingsAsync(rootGroup, this._canceller.Token), this._canceller.Token);
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
        rootGroup.ForEachAll(
            group => { if (group.Id != "/") groupEntities.Add(AwesomeResourceGroupEntity.CreateFrom(group)); },
            resource => resourceEntities.Add(AwesomeResourceEntity.CreateFrom(resource))
        );
        this._groupEntities = groupEntities;
        this._resourceEntities = resourceEntities;

        await tableServiceClient.CreateTableIfNotExistsAsync("groups");
        await tableServiceClient.CreateTableIfNotExistsAsync("resources");
        var tableClients = new TableClients(tableServiceClient);
        foreach (var groupEntity in groupEntities) await tableClients.Groups.AddEntityAsync(groupEntity, cancellationToken);
        foreach (var resourceEntity in resourceEntities) await tableClients.Resources.AddEntityAsync(resourceEntity, cancellationToken);
    }

    internal async ValueTask<IReadOnlyDictionary<string, EmbeddingType>> UpdateEmbeddingsAsync(AwesomeResourceGroup rootGroup, CancellationToken cancellationToken = default)
    {
        var embedder = this._embedder.Value;
        var embeddings = new Dictionary<string, EmbeddingType>();
        rootGroup.ForEachAll(
            g => { if (g.Id != "/") embeddings.Add(g.Id, embedder.Embed<EmbeddingType>(g.Title + "\n" + g.ParagraphsHtml)); },
            r => embeddings.Add(r.Id, embedder.Embed<EmbeddingType>(r.Title + "\n" + r.DescriptionText)),
            cancellationToken
        );
        return await ValueTask.FromResult(embeddings);
    }

    public async ValueTask UpdateVisibiltyBySemanticSearchAsync(AwesomeResourceGroup rootGroup, string searchText, double sensitivity = 0.7)
    {
        // Rsset visibility when the search text is empty.
        if (string.IsNullOrWhiteSpace(searchText))
        {
            rootGroup.ForEachAll(
                g => g.Visible = g.SelectionState != SelectionState.Unselected,
                r => r.Visible = true);
            return;
        }

        if (this._embeddingsTask == null) throw new InvalidOperationException("The embeddings are not ready yet.");
        var embeddings = await this._embeddingsTask;
        var searchEmbedding = this._embedder.Value.Embed<EmbeddingType>(searchText);
        UpdateVisibiltyBySemanticSearch(rootGroup.SubGroups, embeddings, searchEmbedding, sensitivity);
    }

    private static void UpdateVisibiltyBySemanticSearch(IEnumerable<AwesomeResourceGroup> groups, IReadOnlyDictionary<string, EmbeddingType> embeddings, EmbeddingType searchEmbedding, double sensitivity)
    {
        foreach (var group in groups)
        {
            UpdateVisibiltyBySemanticSearch(group, embeddings, searchEmbedding, sensitivity);
        }
    }

    private static void UpdateVisibiltyBySemanticSearch(AwesomeResourceGroup group, IReadOnlyDictionary<string, EmbeddingType> embeddings, EmbeddingType searchEmbedding, double sensitivity)
    {
        group.Visible = group.SelectionState != SelectionState.Unselected;
        if (!group.Visible) return;

        var groupEmbedding = embeddings[group.Id];
        var similarity = groupEmbedding.Similarity(searchEmbedding);
        var groupMatch = similarity > sensitivity;

        foreach (var resource in group.Resources)
        {
            var resourceEmbedding = embeddings[resource.Id];
            resource.Visible = groupMatch || (resourceEmbedding.Similarity(searchEmbedding) > sensitivity);
        }

        UpdateVisibiltyBySemanticSearch(group.SubGroups, embeddings, searchEmbedding, sensitivity);

        group.Visible =
            // (group.ParagraphsHtml != "" && !keywords.Any()) ||
            groupMatch ||
            group.Resources.Any(r => r.Visible) ||
            group.SubGroups.Any(g => g.Visible);

    }

    public async ValueTask DisposeAsync()
    {
        this._canceller.Cancel();
        if (this._savingTask != null) try { await this._savingTask; } catch { }
        if (this._embeddingsTask != null) try { await this._embeddingsTask; } catch { }
        this._canceller.Dispose();
    }
}
