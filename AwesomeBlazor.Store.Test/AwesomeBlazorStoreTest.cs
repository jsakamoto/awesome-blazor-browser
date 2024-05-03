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
                new AwesomeResourceGroup {
                    Id = "/libraries/",
                    Title = "Libraries",
                    Embedding = [0xa0, 0xa1, 0xa2],
                    SubGroups = {
                        new AwesomeResourceGroup{
                            Id = "/libraries/charts/",
                            Title = "Charts",
                            Resources = {
                                new AwesomeResource { Id = "/libraries/charts/blazing-bar-chart/", Title = "Blazing Bar Chart" },
                                new AwesomeResource { Id = "/libraries/charts/blazing-line-chart/", Title = "Blazing Line Chart", Order = 1 },
                            } },
                        new AwesomeResourceGroup {
                            Id = "/libraries/maps/",
                            Title = "Maps",
                            Order = 1,
                            Resources = {
                                new AwesomeResource { Id = "/libraries/maps/blazor-maps/", Title = "Blazor Maps", Embedding = [0xb0, 0xb1] },
                            } } } },
                new AwesomeResourceGroup{
                    Id = "/samples/",
                    Title = "Samples",
                    Order = 1,
                    Resources = {
                        new AwesomeResource { Id = "/samples/blazing-pizza/", Title = "Blazing Pizza" },
                    } } }
        };

        // When
        await testHost.StartAzuriteAsync();
        var store = testHost.GetRequiredService<AwesomeBlazorStore>();
        await store.SaveToTableStorageAsync(rootGroup);

        // Then
        var groups = await DumpAsync<AwesomeResourceGroupEntity>(testHost, "groups", e => $"{e.PartitionKey}|{e.RowKey}|{e.Content}|{Dump(e.Embedding)}");
        groups.Is(
            "%|%libraries%|{\"Order\":0,\"Title\":\"Libraries\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|A0-A1-A2",
            "%|%samples%|{\"Order\":1,\"Title\":\"Samples\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)",
            "%libraries%|%libraries%charts%|{\"Order\":0,\"Title\":\"Charts\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)",
            "%libraries%|%libraries%maps%|{\"Order\":1,\"Title\":\"Maps\",\"TitleHtml\":\"\",\"ParagraphsHtml\":\"\"}|(null)");
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
