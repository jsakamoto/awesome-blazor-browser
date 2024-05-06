using Azure;
using Azure.Data.Tables;
using SmartComponents.LocalEmbeddings;

namespace AwesomeBlazor.Store;

public class AwesomeEmbeddingEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "";

    public required string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    public required byte[] EmbeddingBuff { get; set; }

    public static AwesomeEmbeddingEntity CreateFrom(AwesomeEmbedding embedding)
    {
        static string encode(string value) => value.Replace("/", "%");
        return new AwesomeEmbeddingEntity
        {
            RowKey = encode(embedding.Id),
            EmbeddingBuff = embedding.Embedding.Buffer.ToArray()
        };
    }

    public AwesomeEmbedding ConvertToEmbedding()
    {
        static string decode(string value) => value.Replace("%", "/");
        return new AwesomeEmbedding
        {
            Id = decode(this.RowKey),
            Embedding = new EmbeddingF32(this.EmbeddingBuff)
        };
    }
}
