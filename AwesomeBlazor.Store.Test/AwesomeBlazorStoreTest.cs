using AwesomeBlazor.Models;
using AwesomeBlazor.Store.Test.Fixtures;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;
using SmartComponents.LocalEmbeddings;

namespace AwesomeBlazor.Store.Test;

internal class AwesomeBlazorStoreTest
{
    private static TestHost CreateTestHost() => new(services =>
    {
        var sampleContents = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.md"));
        services.AddSingleton(_ => new TableServiceClient("UseDevelopmentStorage=true"));
        services.AddSingleton(_ => new HttpClient(new TestMessageHandler(sampleContents)));
        services.AddSingleton(_ => new LocalEmbedder());
        services.AddSingleton<AwesomeBlazorStore>();
    });

    [Test]
    public async Task SaveToTableStorageAsync_Test()
    {
        // Given
        await using var testHost = CreateTestHost();

        var rootGroup = new AwesomeResourceGroup
        {
            SubGroups = {
                new AwesomeResourceGroup{
                    Id = "/samples/",
                    Title = "Samples",
                    Resources = {
                        new AwesomeResource { Id = "/samples/blazing-pizza/", Title = "Blazing Pizza" },
                    } },
                new AwesomeResourceGroup {
                    Id = "/libraries/",
                    Title = "Libraries",
                    Order = 1,
                    SubGroups = {
                        new AwesomeResourceGroup {
                            Id = "/libraries/maps/",
                            Title = "Maps",
                            Resources = {
                                new AwesomeResource { Id = "/libraries/maps/blazor-maps/", Title = "Blazor Maps" },
                            } },
                        new AwesomeResourceGroup{
                            Id = "/libraries/charts/",
                            Title = "Charts",
                            Order = 1,
                            Resources = {
                                new AwesomeResource { Id = "/libraries/charts/blazing-line-chart/", Title = "Blazing Line Chart" },
                                new AwesomeResource { Id = "/libraries/charts/blazing-bar-chart/", Title = "Blazing Bar Chart", Order = 1 },
                            } },
                    } },
            }
        };

        // When
        await testHost.StartAzuriteAsync();
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        await store.SaveToTableStorageAsync(rootGroup);

        // Then: groups
        var groups = await testHost.DumpAsync<AwesomeResourceGroupEntity>("groups", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}");
        groups.Is(
            "%|%libraries%|{\"Order\":1,\"Title\":\"Libraries\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}",
            "%|%samples%|{\"Order\":0,\"Title\":\"Samples\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}",
            "%libraries%|%libraries%charts%|{\"Order\":1,\"Title\":\"Charts\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}",
            "%libraries%|%libraries%maps%|{\"Order\":0,\"Title\":\"Maps\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}");

        // Then: resources
        var resources = await testHost.DumpAsync<AwesomeResourceEntity>("resources", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}");
        resources.Is(
            "%libraries%charts%|%libraries%charts%blazing-bar-chart%|{\"Order\":1,\"Title\":\"Blazing Bar Chart\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}",
            "%libraries%charts%|%libraries%charts%blazing-line-chart%|{\"Order\":0,\"Title\":\"Blazing Line Chart\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}",
            "%libraries%maps%|%libraries%maps%blazor-maps%|{\"Order\":0,\"Title\":\"Blazor Maps\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}",
            "%samples%|%samples%blazing-pizza%|{\"Order\":0,\"Title\":\"Blazing Pizza\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}");
    }

    [Test]
    public async Task TryLoadFromTableStorageAsync_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();

        var tableServiceClient = testHost.GetRequiredService<TableServiceClient>();

        var groupEntities = new AwesomeResourceGroupEntity[] {
            new() { PartitionKey = "%", RowKey = "%libraries%", Content = "{\"Order\":1,\"Title\":\"Libraries\"}" },
            new() { PartitionKey = "%", RowKey = "%samples%", Content = "{\"Order\":0,\"Title\":\"Samples\"}" },
            new() { PartitionKey = "%libraries%", RowKey = "%libraries%charts%", Content = "{\"Order\":1,\"Title\":\"Charts\"}" },
            new() { PartitionKey = "%libraries%", RowKey = "%libraries%maps%", Content = "{\"Order\":0,\"Title\":\"Maps\"}" },
        };
        await tableServiceClient.CreateTableAsync("groups");
        var groupTableClient = tableServiceClient.GetTableClient("groups");
        foreach (var groupEntity in groupEntities) { await groupTableClient.AddEntityAsync(groupEntity); }

        var resourceEntities = new AwesomeResourceEntity[] {
            new() { PartitionKey = "%libraries%charts%", RowKey = "%libraries%charts%blazing-bar-chart", Content = "{\"Order\":1,\"Title\":\"Blazing Bar Chart\"}" },
            new() { PartitionKey = "%libraries%charts%", RowKey = "%libraries%charts%blazing-line-chart", Content = "{\"Order\":0,\"Title\":\"Blazing Line Chart\"}" },
            new() { PartitionKey = "%libraries%maps%", RowKey = "%libraries%maps%blazor-maps", Content = "{\"Order\":0,\"Title\":\"Blazor Maps\"}" },
            new() { PartitionKey = "%samples%", RowKey = "%samples%blazing-pizza", Content = "{\"Order\":0,\"Title\":\"Blazing Pizza\"}" },
        };
        await tableServiceClient.CreateTableAsync("resources");
        var resourceTableClient = tableServiceClient.GetTableClient("resources");
        foreach (var resourceEntity in resourceEntities) { await resourceTableClient.AddEntityAsync(resourceEntity); }

        // When
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var rootGroup = await store.TryLoadFromTableStorageAsync();

        // Then
        rootGroup.IsNotNull();

        rootGroup.SubGroups.Select(g => $"{g.Id}|{g.Title}")
            .Is("/samples/|Samples",
                "/libraries/|Libraries");
        var samplesGroup = rootGroup.SubGroups[0];
        samplesGroup.SubGroups.Count.Is(0);
        samplesGroup.Resources.Select(r => $"{r.Id}|{r.Title}")
            .Is("/samples/blazing-pizza|Blazing Pizza");
        var librariesGroup = rootGroup.SubGroups[1];
        librariesGroup.Resources.Count.Is(0);
        librariesGroup.SubGroups.Select(g => $"{g.Id}|{g.Title}")
            .Is("/libraries/maps/|Maps",
                "/libraries/charts/|Charts");
        var mapsSubGroup = librariesGroup.SubGroups[0];
        mapsSubGroup.SubGroups.Count.Is(0);
        mapsSubGroup.Resources.Select(r => $"{r.Id}|{r.Title}")
            .Is("/libraries/maps/blazor-maps|Blazor Maps");
        var chartsSubGroup = librariesGroup.SubGroups[1];
        chartsSubGroup.SubGroups.Count.Is(0);
        chartsSubGroup.Resources.Select(r => $"{r.Id}|{r.Title}")
            .Is("/libraries/charts/blazing-line-chart|Blazing Line Chart",
                "/libraries/charts/blazing-bar-chart|Blazing Bar Chart");
    }

    [Test]
    public async Task TryLoadFromTableStorageAsync_NoGroupTable_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var tableServiceClient = testHost.GetRequiredService<TableServiceClient>();
        await tableServiceClient.CreateTableAsync("resources");

        // When
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var rootGroup = await store.TryLoadFromTableStorageAsync();

        // Then
        rootGroup.IsNull();
    }

    [Test]
    public async Task TryLoadFromTableStorageAsync_NoResourcesTable_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var tableServiceClient = testHost.GetRequiredService<TableServiceClient>();
        await tableServiceClient.CreateTableAsync("groups");

        // When
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var rootGroup = await store.TryLoadFromTableStorageAsync();

        // Then
        rootGroup.IsNull();
    }

    [Test]
    public async Task TryLoadFromTableStorageAsync_NoEntities_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var tableServiceClient = testHost.GetRequiredService<TableServiceClient>();
        await tableServiceClient.CreateTableAsync("groups");
        await tableServiceClient.CreateTableAsync("resources");

        // When
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var rootGroup = await store.TryLoadFromTableStorageAsync();

        // Then
        rootGroup.IsNull();
    }

    [Test]

    public async Task GetAwesomeBlazorContentAsync_Returns_NewInstance_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();

        // When
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var root1 = await store.GetAwesomeBlazorContentAsync();
        var root2 = await store.GetAwesomeBlazorContentAsync();

        // Then
        Object.ReferenceEquals(root1, root2).IsFalse();
    }

    [Test]
    public async Task UpdateEmbeddingsAsync_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var sampleContents = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.md"));
        var rootGroup = AwesomeBlazorParser.ParseMarkdown(sampleContents);

        // When
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var embeddings = await store.UpdateEmbeddingsAsync(rootGroup);

        // Then
        embeddings.Count().Is(16);
        var embedder = testHost.GetRequiredService<LocalEmbedder>();
        var embeddingOfKeyword = embedder.Embed<EmbeddingF32>("Learn about Blazor");
        LocalEmbedder.FindClosest(embeddingOfKeyword, embeddings.Select(e => (e.Key, e.Value)), maxResults: 3)
            .Is("/tutorials/blazor-workshop",
                "/introduction/what-is-blazor/",
                "/awesome-blazor/");
    }

    [Test]
    public async Task UpdateEmbeddingsAsync_SaveAndLoadTableStorage_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var sampleContents = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.md"));
        var tempRoot = AwesomeBlazorParser.ParseMarkdown(sampleContents);

        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        await store.UpdateEmbeddingsAsync(tempRoot);
        await store.SaveToTableStorageAsync(tempRoot);

        // When
        var rootGroup = await store.TryLoadFromTableStorageAsync();
        var embeddings = await store.UpdateEmbeddingsAsync(rootGroup.IsNotNull());

        // Then
        embeddings.Count().Is(16);
        var embedder = testHost.GetRequiredService<LocalEmbedder>();
        var embeddingOfKeyword = embedder.Embed<EmbeddingF32>("Learn about Blazor");
        LocalEmbedder.FindClosest(embeddingOfKeyword, embeddings.Select(e => (e.Key, e.Value)), maxResults: 3)
            .Is("/tutorials/blazor-workshop",
                "/introduction/what-is-blazor/",
                "/awesome-blazor/");
    }

    [Test]
    public async Task UpdateVisibiltyBySemanticSearchAsync_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var rootGroup = await store.GetAwesomeBlazorContentAsync();

        // When
        await store.UpdateVisibiltyBySemanticSearchAsync(rootGroup, "Learn about Blazor");

        // Then
        rootGroup.SubGroups[0].Visible.IsTrue();  // * "/awesome-blazor/"
        rootGroup.SubGroups[0].SubGroups.Count.Is(0);
        rootGroup.SubGroups[0].Resources.Count.Is(0);

        rootGroup.SubGroups[1].Visible.IsFalse(); //   "/special-event---virtual-meetup----today--/"
        rootGroup.SubGroups[1].ForEachAll(
            g => g.Visible.IsFalse(),
            r => r.Visible.IsFalse());
        rootGroup.SubGroups[2].Visible.IsTrue();  // * "/introduction/"

        rootGroup.SubGroups[2].SubGroups[0].Visible.IsTrue();  // * "/introduction/what-is-blazor/"
        rootGroup.SubGroups[2].SubGroups[1].Visible.IsFalse(); //   "/introduction/get-started/"

        rootGroup.SubGroups[3].Visible.IsTrue();  //   "/general/"
        rootGroup.SubGroups[4].Visible.IsTrue();  //   "/sample-projects/"
        rootGroup.SubGroups[5].Visible.IsTrue();  // * "/tutorials/"

        rootGroup.SubGroups[5].Resources[0].Visible.IsTrue();  // * "/tutorials/blazor-workshop"
    }

    [Test]
    public async Task UpdateVisibiltyBySemanticSearchAsync_EmptyText_Test()
    {
        // Given
        await using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        var rootGroup = await store.GetAwesomeBlazorContentAsync();

        // - Make all groups and resources invisible
        rootGroup.ForEachAll(
            g => g.Visible = false,
            r => r.Visible = false);

        // When: with the empty text
        await store.UpdateVisibiltyBySemanticSearchAsync(rootGroup, "");

        // Then: visibilities of all groups and resources are reseted (visible).
        rootGroup.ForEachAll(
            g => g.Visible.IsTrue(),
            r => r.Visible.IsTrue());
    }
}
