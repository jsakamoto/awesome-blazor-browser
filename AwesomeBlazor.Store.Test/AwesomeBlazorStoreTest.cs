using AwesomeBlazor.Models;
using AwesomeBlazor.Store.Test.Fixtures;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeBlazor.Store.Test;

internal class AwesomeBlazorStoreTest
{
    private static TestHost CreateTestHost() => new(services =>
    {
        services.AddSingleton(_ => new TableServiceClient("UseDevelopmentStorage=true"));
        services.AddSingleton<HttpClient>();
        services.AddSingleton<AwesomeBlazorStore>();
    });

    [Test]
    public async Task SaveToTableStorageAsync_Test()
    {
        // Given
        using var testHost = CreateTestHost();

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
                    Embedding = [0xa0, 0xa1, 0xa2],
                    SubGroups = {
                        new AwesomeResourceGroup {
                            Id = "/libraries/maps/",
                            Title = "Maps",
                            Resources = {
                                new AwesomeResource { Id = "/libraries/maps/blazor-maps/", Title = "Blazor Maps", Embedding = [0xb0, 0xb1] },
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
        var groups = await testHost.DumpAsync<AwesomeResourceGroupEntity>("groups", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}|{e.Embedding.Dump()}");
        groups.Is(
            "%|%libraries%|{\"Order\":1,\"Title\":\"Libraries\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|A0-A1-A2",
            "%|%samples%|{\"Order\":0,\"Title\":\"Samples\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)",
            "%libraries%|%libraries%charts%|{\"Order\":1,\"Title\":\"Charts\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)",
            "%libraries%|%libraries%maps%|{\"Order\":0,\"Title\":\"Maps\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)");

        // Then: resources
        var resources = await testHost.DumpAsync<AwesomeResourceEntity>("resources", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}|{e.Embedding.Dump()}");
        resources.Is(
            "%libraries%charts%|%libraries%charts%blazing-bar-chart%|{\"Order\":1,\"Title\":\"Blazing Bar Chart\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|(null)",
            "%libraries%charts%|%libraries%charts%blazing-line-chart%|{\"Order\":0,\"Title\":\"Blazing Line Chart\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|(null)",
            "%libraries%maps%|%libraries%maps%blazor-maps%|{\"Order\":0,\"Title\":\"Blazor Maps\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|B0-B1",
            "%samples%|%samples%blazing-pizza%|{\"Order\":0,\"Title\":\"Blazing Pizza\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|(null)");
    }

    [Test]
    public async Task TryLoadFromTableStorageAsync_Test()
    {
        // Given
        using var testHost = CreateTestHost();
        await testHost.StartAzuriteAsync();

        var tableServiceClient = testHost.GetRequiredService<TableServiceClient>();

        var groupEntities = new AwesomeResourceGroupEntity[] {
            new() { PartitionKey = "%", RowKey = "%libraries%", Content = "{\"Order\":1,\"Title\":\"Libraries\"}", Embedding = [0xc1, 0xc2, 0xc3] },
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
        using var testHost = CreateTestHost();
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
        using var testHost = CreateTestHost();
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
        using var testHost = CreateTestHost();
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
}
