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

    public byte[]? Embedding { get; set; }

    public static AwesomeResourceGroupEntity CreateFrom(AwesomeResourceGroup resourceGroup)
    {
        return new AwesomeResourceGroupEntity
        {
            PartitionKey = resourceGroup.ParentId,
            RowKey = resourceGroup.Id,
            Content = JsonSerializer.Serialize(resourceGroup),
            Embedding = resourceGroup.Embedding
        };
    }

    private static readonly Lazy<MethodInfo?> _idSetter = new(() => typeof(AwesomeResourceGroup).GetProperty(nameof(AwesomeResourceGroup.Id))?.GetSetMethod());

    public AwesomeResourceGroup ConvertToResourceGroup()
    {
        var resourceGroup = JsonSerializer.Deserialize<AwesomeResourceGroup>(this.Content ?? "{}") ?? new();
        _idSetter.Value?.Invoke(resourceGroup, [this.RowKey]);
        resourceGroup.Embedding = this.Embedding;
        return resourceGroup;
    }
}
