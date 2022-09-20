using System.Web;
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

    private bool Loading = true;

    private bool GroupPanelExpanded = false;

    private bool SettingsPanelExpanded = false;

    private readonly TaskCompletionSource ParsingCompletionSource = new();

    protected override async Task OnInitializedAsync()
    {
        var url = "https://raw.githubusercontent.com/AdrienTorris/awesome-blazor/master/README.md";
        var awesomeBlazorContents = await this.HttpClient.GetStringAsync(url);
        this.RootGroup = AwesomeBlazorParser.ParseMarkdown(awesomeBlazorContents);
        this.ParsingCompletionSource.SetResult();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await this.HelperScript.InstallHashWatcherAsync();

        if (this.ParsingCompletionSource.Task.IsCompleted == true && this.Loading == true)
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

    private async Task OnChangeKeywords(string keywords)
    {
        this.Keywords = keywords;

        await this.ParsingCompletionSource.Task;

        this.UpdateRootGroupVisibility();

        var uri = new Uri(this.NavigationManager.Uri);
        var queryStrings = HttpUtility.ParseQueryString(uri.Query);
        queryStrings["k"] = keywords;
        var nextUrl = new UriBuilder(uri) { Query = queryStrings.ToString() }.Uri.ToString();
        this.NavigationManager.NavigateTo(nextUrl, new NavigationOptions() { ReplaceHistoryEntry = true });

        this.GroupPanelExpanded = false;
        this.SettingsPanelExpanded = false;
    }

    private void OnClickGroupPanelMenu()
    {
        this.GroupPanelExpanded = !this.GroupPanelExpanded;
        this.SettingsPanelExpanded = false;
    }

    private void OnClickSettings()
    {
        this.SettingsPanelExpanded = !this.SettingsPanelExpanded;
        this.GroupPanelExpanded = false;
    }

    private void OnClickGroupLink()
    {
        this.GroupPanelExpanded = false;
    }

    private void OnClickMainMask()
    {
        this.GroupPanelExpanded = false;
        this.SettingsPanelExpanded = false;
    }
}
