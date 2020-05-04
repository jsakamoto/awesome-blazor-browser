using System;
using System.Linq;

namespace AwesomeBlazor.Models
{
    public static class AwesomeResourceFilter
    {
        public static void UpdateVisibiltyByKeywordFilter(this AwesomeResourceGroup group, string keywords)
        {
            var keywordsArray = string.IsNullOrEmpty(keywords) ?
                new string[0] :
                keywords.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            group.UpdateVisibiltyByKeywordFilter(keywordsArray);
        }

        private static void UpdateVisibiltyByKeywordFilter(this AwesomeResourceGroup group, string[] keywords)
        {
            group.Visible = group.Selected;
            if (group.Visible)
            {
                foreach (var resource in group.Resources)
                {
                    resource.Visible = keywords.All(keyword =>
                        resource.Title.ToLower().Contains(keyword) ||
                        resource.DescriptionText.ToLower().Contains(keyword));
                }

                foreach (var subGroup in group.SubGroups)
                {
                    subGroup.UpdateVisibiltyByKeywordFilter(keywords);
                }

                group.Visible =
                    (group.ParagraphsHtml != "" && !keywords.Any()) ||
                    group.Resources.Any(r => r.Visible) ||
                    group.SubGroups.Any(g => g.Visible);
            }
        }
    }
}
