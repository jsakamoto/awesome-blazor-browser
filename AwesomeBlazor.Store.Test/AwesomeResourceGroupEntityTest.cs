using AwesomeBlazor.Models;

namespace AwesomeBlazor.Store.Test;

internal class AwesomeResourceGroupEntityTest
{
    [Test]
    public void CreateFrom_Test()
    {
        // Given
        var resourceGroup = new AwesomeResourceGroup
        {
            Id = "/hello-world-/",
            Title = "Hello, World!",
            TitleHtml = "<p>Hello, World!</p>",
            Embedding = [0x01, 0x02, 0x03],
            ParagraphsHtml = "<p>Nice to meet you.</p>"
        };

        // When
        var entity = AwesomeResourceGroupEntity.CreateFrom(resourceGroup);

        // Then
        entity.PartitionKey.Is("/");
        entity.RowKey.Is("/hello-world-/");
        entity.Embedding.Is([0x01, 0x02, 0x03]);
        entity.Content.Is("{" +
            "\"Title\":\"Hello, World!\"," +
            "\"TitleHtml\":\"\\u003Cp\\u003EHello, World!\\u003C/p\\u003E\"," +
            "\"ParagraphsHtml\":\"\\u003Cp\\u003ENice to meet you.\\u003C/p\\u003E\"" +
        "}");
    }
}
