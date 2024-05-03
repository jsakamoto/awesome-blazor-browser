using System.Reflection;
using System.Text.Json;
using AwesomeBlazor.Models;
using Azure;
using Azure.Data.Tables;

namespace AwesomeBlazor.Store;

public class AwesomeResourceEntity : ITableEntity
{
    public required string PartitionKey { get; set; }

    public required string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    public string? Content { get; set; }

    public byte[]? Embedding { get; set; }

    public static AwesomeResourceEntity CreateFrom(AwesomeResource resource)
    {
        static string encode(string value) => value.Replace("/", "%");
        return new AwesomeResourceEntity
        {
            PartitionKey = encode(resource.ParentId),
            RowKey = encode(resource.Id),
            Content = JsonSerializer.Serialize(resource),
            Embedding = resource.Embedding
        };
    }

    private static readonly Lazy<MethodInfo?> _idSetter = new(() => typeof(AwesomeResource).GetProperty(nameof(AwesomeResource.Id))?.GetSetMethod());

    public AwesomeResource ConvertToResource()
    {
        static string decode(string value) => value.Replace("%", "/");
        var resourceGroup = JsonSerializer.Deserialize<AwesomeResource>(this.Content ?? "{}") ?? new();
        _idSetter.Value?.Invoke(resourceGroup, [decode(this.RowKey)]);
        resourceGroup.Embedding = this.Embedding;
        return resourceGroup;
    }
}
