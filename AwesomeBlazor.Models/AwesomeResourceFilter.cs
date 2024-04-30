namespace AwesomeBlazor.Models;

public static class AwesomeResourceFilter
{
    public static void UpdateVisibiltyByKeywordFilter(this IEnumerable<AwesomeResourceGroup> groups, string keywords)
    {
        var keywordsArray = string.IsNullOrEmpty(keywords) ?
            [] :
            keywords.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        groups.UpdateVisibiltyByKeywordFilter(keywordsArray);
    }

    private static void UpdateVisibiltyByKeywordFilter(this IEnumerable<AwesomeResourceGroup> groups, string[] keywords)
    {
        foreach (var group in groups)
        {
            group.UpdateVisibiltyByKeywordFilter(keywords);
        }
    }

    private static void UpdateVisibiltyByKeywordFilter(this AwesomeResourceGroup group, string[] keywords)
    {
        group.Visible = group.SelectionState != SelectionState.Unselected;
        if (group.Visible)
        {
            foreach (var resource in group.Resources)
            {
                resource.Visible = keywords.All(keyword =>
                    resource.Title.ToLower().Contains(keyword) ||
                    resource.DescriptionText.ToLower().Contains(keyword) ||
                    group.Title.ToLower().Contains(keyword));
            }

            group.SubGroups.UpdateVisibiltyByKeywordFilter(keywords);

            group.Visible =
                (group.ParagraphsHtml != "" && !keywords.Any()) ||
                group.Resources.Any(r => r.Visible) ||
                group.SubGroups.Any(g => g.Visible);
        }
    }
}
