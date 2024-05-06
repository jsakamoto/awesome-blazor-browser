using AwesomeBlazor.Models;

namespace AwesomeBlazor.Store.Test;

internal class AwesomeResourceEntityTest
{
    [Test]
    public void CreateFrom_Test()
    {
        // Given
        var resource = new AwesomeResource
        {
            Id = "/hello/world",
            Order = 34,
            Title = "Hello, World!",
            ResourceUrl = "https://example.com/hello",
            GitHubStarsUrl = "https://example.com/github/stars",
            LastCommitUrl = "https://example.com/commits/latest",
            DescriptionText = "Nice to meet you.",
            DescriptionHtml = "<p>Nice to meet you.</p>"
        };

        // When
        var entity = AwesomeResourceEntity.CreateFrom(resource);

        // Then
        entity.PartitionKey.Is("%hello%");
        entity.RowKey.Is("%hello%world");
        entity.Content.Is("{" +
            "\"Order\":34," +
            "\"Title\":\"Hello, World!\"," +
            "\"ResourceUrl\":\"https://example.com/hello\"," +
            "\"GitHubStarsUrl\":\"https://example.com/github/stars\"," +
            "\"LastCommitUrl\":\"https://example.com/commits/latest\"," +
            "\"DescriptionText\":\"Nice to meet you.\"," +
            "\"DescriptionHtml\":\"\\u003Cp\\u003ENice to meet you.\\u003C/p\\u003E\"" +
        "}");
    }

    [Test]
    public void ConvertTo_Test()
    {
        // Given
        var entity = new AwesomeResourceEntity
        {
            PartitionKey = "%foo%bar%",
            RowKey = "%foo%bar%fizz",
            Content = "{" +
                "\"Order\":56," +
                "\"Title\":\"Fizz Buzz!\"," +
                "\"ResourceUrl\":\"https://example.com/fizz-buzz\"," +
                "\"GitHubStarsUrl\":\"https://example.com/github/stars\"," +
                "\"LastCommitUrl\":\"https://example.com/fizz-buzz/commits/latest\"," +
                "\"DescriptionText\":\"You are welcome.\"," +
                "\"DescriptionHtml\":\"\\u003Cdiv\\u003EYou are welcome.\\u003C/div\\u003E\"" +
            "}"
        };

        // When
        var resource = entity.ConvertToResource();

        // Then
        resource.ParentId.Is("/foo/bar/");
        resource.Id.Is("/foo/bar/fizz");
        resource.Order.Is(56);
        resource.Title.Is("Fizz Buzz!");
        resource.ResourceUrl.Is("https://example.com/fizz-buzz");
        resource.GitHubStarsUrl.Is("https://example.com/github/stars");
        resource.LastCommitUrl.Is("https://example.com/fizz-buzz/commits/latest");
        resource.DescriptionText.Is("You are welcome.");
        resource.DescriptionHtml.Is("<div>You are welcome.</div>");
    }
}
