using Azure.Data.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeBlazor.Store.Test.Fixtures;

internal static class ExtensionsToDump
{
    /// <summary>
    /// Converts a byte array to a string representation.
    /// </summary>
    internal static string Dump(this byte[]? bytes) => bytes is null ? "(null)" : BitConverter.ToString(bytes);

    /// <summary>
    /// Asynchronously dumps the entities from a table and returns a collection of string representations.
    /// </summary>
    internal static async ValueTask<IEnumerable<string>> DumpAsync<TEntity>(this IServiceProvider services, string tableName, Func<TEntity, string> dump)
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
