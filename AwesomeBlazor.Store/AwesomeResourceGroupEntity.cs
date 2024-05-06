using System.Reflection;
using System.Text.Json;
using AwesomeBlazor.Models;
using Azure;
using Azure.Data.Tables;

namespace AwesomeBlazor.Store;

public class AwesomeResourceGroupEntity : ITableEntity
{
    public required string PartitionKey { get; set; }

    public required string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    public string? Content { get; set; }

    public static AwesomeResourceGroupEntity CreateFrom(AwesomeResourceGroup resourceGroup)
    {
        static string encode(string value) => value.Replace("/", "%");

        return new AwesomeResourceGroupEntity
        {
            PartitionKey = encode(resourceGroup.ParentId),
            RowKey = encode(resourceGroup.Id),
            Content = JsonSerializer.Serialize(resourceGroup)
        };
    }

    private static readonly Lazy<MethodInfo?> _idSetter = new(() => typeof(AwesomeResourceGroup).GetProperty(nameof(AwesomeResourceGroup.Id))?.GetSetMethod());

    public AwesomeResourceGroup ConvertToResourceGroup()
    {
        static string decode(string value) => value.Replace("%", "/");
        var resourceGroup = JsonSerializer.Deserialize<AwesomeResourceGroup>(this.Content ?? "{}") ?? new();
        _idSetter.Value?.Invoke(resourceGroup, [decode(this.RowKey)]);
        return resourceGroup;
    }
}
