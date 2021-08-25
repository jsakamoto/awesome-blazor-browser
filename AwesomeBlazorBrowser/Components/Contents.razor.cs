using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components
{
    public partial class Contents
    {
        [Parameter]
        public string ParentGroupAnchor { get; set; } = "";

        [Parameter]
        public IEnumerable<AwesomeResourceGroup> Groups { get; set; } = Enumerable.Empty<AwesomeResourceGroup>();

        private bool HasBadges(AwesomeResource resource)
        {
            return resource.GitHubStarsUrl != "" || resource.LastCommitUrl != "";
        }
    }
}
