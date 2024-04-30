using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Markdig;

namespace AwesomeBlazor.Models;

public static class AwesomeBlazorParser
{
    private enum LineTypes
    {
        Group,
        Paragraph,
        Resource
    }

    public static AwesomeResourceGroup ParseMarkdown(string markdownContents, AwesomeBlazorParserOptions? options = null)
    {
        options ??= new AwesomeBlazorParserOptions();

        var currentGroup = default(AwesomeResourceGroup);
        var awsomeBlazorRoot = new AwesomeResourceGroup();
        var groups = new[] { awsomeBlazorRoot };

        var paragraphs = new List<string>();

        var prevLineType = LineTypes.Group;
        var lines = markdownContents.Split('\r', '\n');
        foreach (var current in lines)
        {
            var groupMatch = Regex.Match(current, "^(?<level>#+)[ ]*(?<title>.+)$");
            if (groupMatch.Success)
            {
                prevLineType = LineTypes.Group;
                var title = groupMatch.Groups["title"].Value;
                var level = Math.Max(1, groupMatch.Groups["level"].Value.Length - 1);
                if (groups.Length <= level) Array.Resize(ref groups, level + 1);

                if (title == options.EndCategoryName) break;

                if (currentGroup != null && paragraphs.Any(p => !string.IsNullOrEmpty(p)))
                {
                    currentGroup.ParagraphsHtml = PostProcessHtml(Markdown.ToHtml(string.Join("\n", paragraphs)), stripParagraphTag: false);
                }
                paragraphs.Clear();

                if (options.SkipCategoryNames.Contains(title))
                {
                    currentGroup = null;
                }
                else
                {
                    var titleHtml = PostProcessHtml(Markdown.ToHtml(title), stripParagraphTag: true);
                    currentGroup = new AwesomeResourceGroup(title: HtmlToText(titleHtml), titleHtml: titleHtml);
                    groups[level - 1].SubGroups.Add(currentGroup);
                    groups[level] = currentGroup;
                }
            }

            else if (currentGroup != null)
            {
                if (TryParseAsResource(current, out var resource))
                {
                    prevLineType = LineTypes.Resource;
                    currentGroup.Resources.Add(resource);
                }
                else if (prevLineType == LineTypes.Resource && !string.IsNullOrEmpty(current))
                {
                    prevLineType = LineTypes.Resource;
                    var descriptionHtml = PostProcessHtml(Markdown.ToHtml(current), stripParagraphTag: true);
                    var descriptionText = HtmlToText(descriptionHtml);
                    var lastResource = currentGroup.Resources.Last();
                    lastResource.DescriptionHtml += " " + descriptionHtml;
                    lastResource.DescriptionText += " " + descriptionText;
                }
                else
                {
                    prevLineType = LineTypes.Paragraph;
                    paragraphs.Add(current);
                }
            }
        }

        return awsomeBlazorRoot;
    }

    private static string PostProcessHtml(string html, bool stripParagraphTag)
    {
        if (stripParagraphTag)
        {
            html = Regex.Replace(html, "</?p>", "");
        }
        return Regex.Replace(html, "<a (?<prop>[^>]+)>", m => "<a " + m.Groups["prop"].Value + " target=\"_blank\">")
            .Trim();
    }

    private static string HtmlToText(string html)
    {
        return Regex.Replace(html, "<[^>]+>", "")
            .Replace("&quot;", "\"")
            .Replace("&amp;", "&")
            .Trim();
    }

    internal static bool TryParseAsResource(string contents, [NotNullWhen(true)] out AwesomeResource? resource)
    {
        resource = null;

        if (string.IsNullOrEmpty(contents)) return false;

        var m1 = Regex.Match(contents, @"^(\*[ ]+)?\[(?<title>[^]]+)\]\((?<url>[^)]+)\)[ ]+(\-[ ]+)?(?<body>.+)$");
        if (!m1.Success) return false;

        var body = m1.Groups["body"].Value;
        var gitHubStarsUrl = "";
        var lastCommitUrl = "";
        body = Regex.Replace(body, @"\!\[[^]]+\]\((?<url>https://img.shields.io/github/(?<type>last-commit|stars)/[^)]+)\)", m =>
        {
            if (m.Groups["type"].Value == "stars")
                gitHubStarsUrl = m.Groups["url"].Value;
            else
                lastCommitUrl = m.Groups["url"].Value;
            return "";
        });


        var descriptionHtml = PostProcessHtml(Markdown.ToHtml(body), stripParagraphTag: true);
        var descriptionText = HtmlToText(descriptionHtml);
        resource = new AwesomeResource(
            m1.Groups["title"].Value,
            m1.Groups["url"].Value,
            gitHubStarsUrl,
            lastCommitUrl,
            descriptionText,
            descriptionHtml
        );
        return true;
    }
}
