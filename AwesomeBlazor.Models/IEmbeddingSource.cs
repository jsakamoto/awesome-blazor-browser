namespace AwesomeBlazor.Models;

public interface IEmbeddingSource
{
    string Id { get; }

    byte[]? Embedding { get; set; }
}
