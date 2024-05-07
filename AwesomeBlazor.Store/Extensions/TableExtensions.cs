using Azure.Data.Tables;

namespace AwesomeBlazor.Store.Extensions;

internal static class TableExtensions
{
    /// <summary>
    /// Returns a value indicating whether the table exists.
    /// </summary>
    public static async ValueTask<bool> ExistTableAsync(this TableServiceClient tableServiceClient, string tableName)
    {
        return await tableServiceClient.QueryAsync($"TableName eq \'{tableName}\'")
            .GetAsyncEnumerator()
            .MoveNextAsync();
    }

    /// <summary>
    /// Returns all entities as a list.
    /// </summary>
    public static async ValueTask<List<T>> ToListAsync<T>(this TableClient tableClient) where T : class, ITableEntity
    {
        var list = new List<T>();
        await foreach (var entity in tableClient.QueryAsync<T>())
        {
            list.Add(entity);
        }
        return list;
    }
}
