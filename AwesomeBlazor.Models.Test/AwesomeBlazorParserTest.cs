namespace AwesomeBlazor.Models.Test;

public class AwesomeBlazorParserTest
{
    [Test]
    public void Parse_Test()
    {
        var contents = TestFixture.GetContentsForTest();
        var root = AwesomeBlazorParser.ParseMarkdown(contents);

        root.Resources.Any().IsFalse();
        root.SubGroups.Select(g => g.Title)
            .Is("Awesome Blazor",
                "Special event: \"Virtual Meetup\". [TODAY!]",
                "Introduction", "General", "Sample Projects", "Tutorials");

        var topTitle = root.SubGroups[0];
        topTitle.SelectionState.Is(SelectionState.Selected);
        topTitle.Resources.Any().IsFalse();
        topTitle.SubGroups.Any().IsFalse();
        topTitle.ParagraphsHtml.IsNot("");

        var specialEvent = root.SubGroups[1];
        specialEvent.SelectionState.Is(SelectionState.Selected);
        specialEvent.Resources.Any().IsFalse();
        specialEvent.SubGroups.Any().IsFalse();
        specialEvent.ParagraphsHtml.IsNot("");

        var introduction = root.SubGroups[2];
        introduction.SelectionState.Is(SelectionState.Selected);
        introduction.ParagraphsHtml.Is("");
        introduction.Resources.Any().IsFalse();
        introduction.SubGroups.Select(g => g.Title)
            .Is("What is Blazor", "Get started");

        var whatsBlazor = introduction.SubGroups[0];
        whatsBlazor.SelectionState.Is(SelectionState.Selected);
        whatsBlazor.Resources.Any().IsFalse();
        whatsBlazor.SubGroups.Any().IsFalse();
        whatsBlazor.ParagraphsHtml.IsNot("");

        var getStarted = introduction.SubGroups[1];
        getStarted.SelectionState.Is(SelectionState.Selected);
        getStarted.Resources.Any().IsFalse();
        getStarted.SubGroups.Any().IsFalse();
        getStarted.ParagraphsHtml.IsNot("");

        var general = root.SubGroups[3];
        general.SelectionState.Is(SelectionState.Selected);
        general.SubGroups.Any().IsFalse();
        general.ParagraphsHtml.Is("");
        general.Resources.Select(r => $"{r.Title} | {r.ResourceUrl}")
            .Is("ASP.NET Blog's archives | https://devblogs.microsoft.com/aspnet/category/blazor/",
                "eShopOnBlazor | https://github.com/dotnet-architecture/eShopOnBlazor");

        var sampleProjects = root.SubGroups[4];
        sampleProjects.SelectionState.Is(SelectionState.Selected);
        sampleProjects.ParagraphsHtml.Is("");
        sampleProjects.Resources.Any().IsFalse();
        sampleProjects.SubGroups.Select(g => g.Title)
            .Is("Authentication", "Cloud");

        var authentication = sampleProjects.SubGroups[0];
        authentication.SelectionState.Is(SelectionState.Selected);
        authentication.SubGroups.Any().IsFalse();
        authentication.ParagraphsHtml.Is("");
        authentication.Resources.Select(r => $"{r.Title} | {r.ResourceUrl}")
            .Is("BlazorBoilerplate | https://github.com/enkodellc/blazorboilerplate");

        var cloud = sampleProjects.SubGroups[1];
        cloud.SelectionState.Is(SelectionState.Selected);
        cloud.SubGroups.Any().IsFalse();
        cloud.ParagraphsHtml.Is("");
        cloud.Resources.Select(r => $"{r.Title} | {r.ResourceUrl}")
            .Is("BlazorAzure.WebApp | https://github.com/gpeipman/BlazorDemo/tree/master/BlazorAzure.WebApp",
                "BlazorFile2Azure | https://github.com/daltskin/BlazorFile2Azure");

        var tutorials = root.SubGroups[5];
        tutorials.SelectionState.Is(SelectionState.Selected);
        tutorials.SubGroups.Any().IsFalse();
        tutorials.ParagraphsHtml.Is("");
        tutorials.Resources.Select(r => $"{r.Title} | {r.ResourceUrl}")
            .Is("Blazor workshop | https://github.com/dotnet-presentations/blazor-workshop/");
    }

    [Test]
    public void ParseAsResource_Simplest_Test()
    {
        AwesomeBlazorParser.TryParseAsResource(
            "* [Fizz Buzz](https://github.com/fizz/buzz) - " +
            "This is fizz buzz. ",
            out var resource
        ).IsTrue();

        resource.IsNotNull();
        resource.Title.Is("Fizz Buzz");
        resource.ResourceUrl.Is("https://github.com/fizz/buzz");
        resource.GitHubStarsUrl.Is("");
        resource.LastCommitUrl.Is("");
        resource.DescriptionText.Is("This is fizz buzz.");
    }

    [Test]
    public void ParseAsResource_FullSet_Test()
    {
        AwesomeBlazorParser.TryParseAsResource(
            "* [Foo Bar](https://github.com/foo/bar) - " +
            "![GitHub stars](https://img.shields.io/github/stars/foo/bar?style=flat-square&cacheSeconds=604800&logo=foo) " +
            "![last commit](https://img.shields.io/github/last-commit/foo/bar?style=flat-square&cacheSeconds=86400) " +
            "![custom badge](https://img.shields.io/foo) This is `foo` and <bar>. " +
            "[Demo](https://foo.bar.example.com) and [[Video]](https://youtube.com/).",
            out var resource
        ).IsTrue();

        resource.IsNotNull();
        resource.Title.Is("Foo Bar");
        resource.ResourceUrl.Is("https://github.com/foo/bar");
        resource.GitHubStarsUrl.Is("https://img.shields.io/github/stars/foo/bar?style=flat-square&cacheSeconds=604800&logo=foo");
        resource.LastCommitUrl.Is("https://img.shields.io/github/last-commit/foo/bar?style=flat-square&cacheSeconds=86400");
        resource.DescriptionText.Is("This is foo and . Demo and [Video].");
        resource.DescriptionHtml.Is(
            "<img src=\"https://img.shields.io/foo\" alt=\"custom badge\" /> " +
            "This is <code>foo</code> and <bar>. " +
            "<a href=\"https://foo.bar.example.com\" target=\"_blank\">Demo</a> and " +
            "<a href=\"https://youtube.com/\" target=\"_blank\">[Video]</a>.");
    }

    [Test]
    public void ParseAsResource_with_Missing_Hyphen_Test()
    {
        AwesomeBlazorParser.TryParseAsResource(
            "* [Fizz Buzz](https://github.com/fizz/buzz) " +
            "This is fizz buzz. ",
            out var resource
        ).IsTrue();

        resource.IsNotNull();
        resource.Title.Is("Fizz Buzz");
        resource.ResourceUrl.Is("https://github.com/fizz/buzz");
        resource.GitHubStarsUrl.Is("");
        resource.LastCommitUrl.Is("");
        resource.DescriptionText.Is("This is fizz buzz.");
    }

    [Test]
    public void ParseAsResource_with_Missing_Bullet_Test()
    {
        AwesomeBlazorParser.TryParseAsResource(
            "[Fizz Buzz](https://github.com/fizz/buzz) " +
            "This is fizz buzz. ",
            out var resource
        ).IsTrue();

        resource.IsNotNull();
        resource.Title.Is("Fizz Buzz");
        resource.ResourceUrl.Is("https://github.com/fizz/buzz");
        resource.GitHubStarsUrl.Is("");
        resource.LastCommitUrl.Is("");
        resource.DescriptionText.Is("This is fizz buzz.");
    }
}
