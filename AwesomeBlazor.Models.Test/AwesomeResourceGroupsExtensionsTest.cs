namespace AwesomeBlazor.Models.Test;

public class AwesomeResourceGroupsExtensionsTest
{
    [Test]
    public void EnumGroupsDescendants_Test()
    {
        var contents = TestFixture.GetContentsForTest();
        var root = AwesomeBlazorParser.ParseMarkdown(contents);

        root.SubGroups.EnumGroupsDescendants()
            .Select(g => g.Title)
            .Is(
                "Awesome Blazor",
                "Special event: \"Virtual Meetup\". [TODAY!]",
                "Introduction",
                    "What is Blazor",
                    "Get started",
                "General",
                "Sample Projects",
                    "Authentication",
                    "Cloud",
                "Tutorials");
    }
}
