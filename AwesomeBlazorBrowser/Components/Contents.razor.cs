using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components;

public partial class Contents
{
    [Inject] public HelperScriptService HelperScript { get; init; } = null!;

    [Parameter]
    public string ParentGroupAnchor { get; set; } = "";

    [Parameter]
    public IEnumerable<AwesomeResourceGroup> Groups { get; set; } = Enumerable.Empty<AwesomeResourceGroup>();

    private bool HasBadges(AwesomeResource resource)
    {
        return resource.GitHubStarsUrl != "" || resource.LastCommitUrl != "";
    }

    private async Task OnClickGroupLink(string anchorName)
    {
        await this.HelperScript.ScrollToAnchorAsync(anchorName, smooth: true, changeUrl: true);
    }
}
