using AwesomeBlazor.Models;
using AwesomeBlazor.Store.Test.Fixtures;
using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeBlazor.Store.Test;

internal class AwesomeBlazorStoreTest
{
    [Test]
    public async Task SaveToTableStorageAsync_Test()
    {
        // Given
        using var testHost = new TestHost(services =>
        {
            services.AddSingleton(_ => new TableServiceClient("UseDevelopmentStorage=true"));
            services.AddSingleton<HttpClient>();
            services.AddSingleton<AwesomeBlazorStore>();
        });

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
        var groups = await DumpAsync<AwesomeResourceGroupEntity>(testHost, "groups", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}|{Dump(e.Embedding)}");
        groups.Is(
            "%|%libraries%|{\"Order\":1,\"Title\":\"Libraries\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|A0-A1-A2",
            "%|%samples%|{\"Order\":0,\"Title\":\"Samples\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)",
            "%libraries%|%libraries%charts%|{\"Order\":1,\"Title\":\"Charts\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)",
            "%libraries%|%libraries%maps%|{\"Order\":0,\"Title\":\"Maps\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)");

        // Then: resources
        var resources = await DumpAsync<AwesomeResourceEntity>(testHost, "resources", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}|{Dump(e.Embedding)}");
        resources.Is(
            "%libraries%charts%|%libraries%charts%blazing-bar-chart%|{\"Order\":1,\"Title\":\"Blazing Bar Chart\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|(null)",
            "%libraries%charts%|%libraries%charts%blazing-line-chart%|{\"Order\":0,\"Title\":\"Blazing Line Chart\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|(null)",
            "%libraries%maps%|%libraries%maps%blazor-maps%|{\"Order\":0,\"Title\":\"Blazor Maps\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|B0-B1",
            "%samples%|%samples%blazing-pizza%|{\"Order\":0,\"Title\":\"Blazing Pizza\",\"ResourceUrl\":\"\",\"GitHubStarsUrl\":\"\",\"LastCommitUrl\":\"\",\"DescriptionText\":\"\",\"DescriptionHtml\":\"\"}|(null)");
    }

    private static string Dump(byte[]? bytes) => bytes is null ? "(null)" : BitConverter.ToString(bytes);

    private static async ValueTask<IEnumerable<string>> DumpAsync<TEntity>(IServiceProvider services, string tableName, Func<TEntity, string> dump)
        where TEntity : class, ITableEntity
    {
        var tableClient = services.GetRequiredService<TableServiceClient>().GetTableClient(tableName);
        var results = new List<string>();
        await foreach (var entity in tableClient.QueryAsync<TEntity>())
        {
            results.Add(dump(entity));
        }
        return results;
    }
}
