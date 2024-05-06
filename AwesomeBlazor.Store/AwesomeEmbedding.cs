using SmartComponents.LocalEmbeddings;

namespace AwesomeBlazor.Store;

public class AwesomeEmbedding
{
    public required string Id { get; set; }

    public EmbeddingF32 Embedding { get; set; }
}
