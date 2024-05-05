using Azure.Data.Tables;

namespace AwesomeBlazor.Store.Extensions;

internal static class TableExtensions
{
    /// <summary>
    /// Returns a value indicating whether the table exists.
    /// </summary>
    public static async ValueTask<bool> ExistTableAsync(this TableServiceClient tableServiceClient, string tableName)
    {
        return await tableServiceClient.QueryAsync($"TableName eq \"{tableName}\"")
            .GetAsyncEnumerator()
            .MoveNextAsync();
    }
}
