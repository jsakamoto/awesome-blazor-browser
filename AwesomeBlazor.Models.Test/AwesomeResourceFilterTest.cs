namespace AwesomeBlazor.Models.Test;

public class AwesomeResourceFilterTest
{
    [Test]
    public void UpdateVisibiltyByKeywordFilter_Hit_in_Description_Test()
    {
        var contents = TestFixture.GetContentsForTest();
        var root = AwesomeBlazorParser.ParseMarkdown(contents);

        root.SubGroups.UpdateVisibiltyByKeywordFilter(".net");
        var visibleGroups = root.SubGroups.Where(g => g.Visible).ToArray();
        visibleGroups.Select(g => g.Title)
            .Is("General", "Tutorials");

        var general = visibleGroups[0];
        general.Resources
            .Where(r => r.Visible)
            .Select(r => r.Title)
            .Is("ASP.NET Blog's archives", "eShopOnBlazor");

        var tutorials = visibleGroups[1];
        tutorials.Resources
            .Where(r => r.Visible)
            .Select(r => r.Title)
            .Is("Blazor workshop");
    }

    [Test]
    public void UpdateVisibiltyByKeywordFilter_Hit_in_Level2_Title_Test()
    {
        var contents = TestFixture.GetContentsForTest();
        var root = AwesomeBlazorParser.ParseMarkdown(contents);

        root.SubGroups.UpdateVisibiltyByKeywordFilter("file2");
        var visibleGroups = root.SubGroups.Where(g => g.Visible).ToArray();
        visibleGroups
            .Select(g => g.Title)
            .Is("Sample Projects");

        var sampleProject = visibleGroups[0];
        sampleProject.SubGroups
            .Where(g => g.Visible)
            .Select(g => g.Title)
            .Is("Cloud");

        var cloud = sampleProject.SubGroups.First(g => g.Visible);
        cloud.Resources
            .Where(r => r.Visible)
            .Select(r => r.Title)
            .Is("BlazorFile2Azure");
    }

    [Test]
    public void UpdateVisibiltyByKeywordFilter_Hit_MultiKeywords_Test()
    {
        var contents = TestFixture.GetContentsForTest();
        var root = AwesomeBlazorParser.ParseMarkdown(contents);

        root.SubGroups.UpdateVisibiltyByKeywordFilter(".net shop microsoft");
        var visibleGroups = root.SubGroups.Where(g => g.Visible).ToArray();
        visibleGroups
            .Select(g => g.Title)
            .Is("General");

        var general = visibleGroups[0];
        general.Resources
            .Where(r => r.Visible)
            .Select(r => r.Title)
            .Is("eShopOnBlazor");
    }
}
