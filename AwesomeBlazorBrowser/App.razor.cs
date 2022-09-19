using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser;

public partial class App
{
    [Inject] HttpClient HttpClient { get; init; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    [Inject] public HelperScriptService HelperScript { get; init; } = null!;

    private AwesomeResourceGroup RootGroup = new();

    private string Keywords = "";

    private bool ParsingComplete = false;

    private bool Loading = true;

    private bool GroupPanelExpanded = false;

    protected override async Task OnInitializedAsync()
    {
        var url = "https://raw.githubusercontent.com/AdrienTorris/awesome-blazor/master/README.md";
        var awesomeBlazorContents = await this.HttpClient.GetStringAsync(url);
        this.RootGroup = AwesomeBlazorParser.ParseMarkdown(awesomeBlazorContents);
        this.ParsingComplete = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.ParsingComplete == true && this.Loading == true)
        {
            this.Loading = false;
            this.StateHasChanged();

            var uriFragment = new Uri(this.NavigationManager.Uri).Fragment;
            if (uriFragment != "")
            {
                await this.HelperScript.ScrollToAnchorAsync(uriFragment, smooth: false);
            }
        }
    }

    private void UpdateRootGroupVisibility()
    {
        this.RootGroup.SubGroups.UpdateVisibiltyByKeywordFilter(this.Keywords);
    }

    private void OnChangeGroupState()
    {
        this.UpdateRootGroupVisibility();
    }

    private void OnChangeKeywords(string keywords)
    {
        this.Keywords = keywords;
        this.UpdateRootGroupVisibility();
    }

    private void OnClickGroupPanelMenu()
    {
        this.GroupPanelExpanded = !this.GroupPanelExpanded;
    }

    private void OnClickGroupLink()
    {
        this.GroupPanelExpanded = false;
    }
}
