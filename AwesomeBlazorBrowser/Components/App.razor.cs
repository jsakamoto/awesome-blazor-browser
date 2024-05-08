using System.Web;
using AwesomeBlazor.Models;
using AwesomeBlazor.Store;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components;

public partial class App
{
    [Inject] AwesomeBlazorStore Store { get; init; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    [Inject] public HelperScriptService HelperScript { get; init; } = null!;

    private Task<AwesomeResourceGroup> GettingContentsTask = Task.FromResult<AwesomeResourceGroup>(new());

    private AwesomeResourceGroup RootGroup = new();

    private string Keywords = "";

    private bool Loading = true;

    private bool InitialScrolled = false;

    private bool GroupPanelExpanded = false;

    private bool SettingsPanelExpanded = false;

    protected override async Task OnInitializedAsync()
    {
        this.GettingContentsTask = this.Store.GetAwesomeBlazorContentAsync().AsTask();
        this.RootGroup = await this.GettingContentsTask;
        this.Loading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await this.HelperScript.InstallHashWatcherAsync();

        if (this.GettingContentsTask.IsCompleted && this.InitialScrolled == false)
        {
            this.InitialScrolled = true;
            var uriFragment = new Uri(this.NavigationManager.Uri).Fragment;
            if (uriFragment != "")
            {
                await this.HelperScript.ScrollToAnchorAsync(uriFragment, smooth: false);
            }
        }
    }

    private async ValueTask UpdateRootGroupVisibilityAsync()
    {
        await this.Store.UpdateVisibiltyBySemanticSearchAsync(this.RootGroup, this.Keywords, sensitivity: 0.58);
    }

    private async Task OnChangeGroupStateAsync()
    {
        await this.UpdateRootGroupVisibilityAsync();
    }

    private async Task OnChangeKeywords(string keywords)
    {
        this.Keywords = keywords;

        await this.GettingContentsTask;

        await this.UpdateRootGroupVisibilityAsync();

        var uri = new Uri(this.NavigationManager.Uri);
        var queryStrings = HttpUtility.ParseQueryString(uri.Query);
        queryStrings["k"] = keywords;
        var nextUrl = new UriBuilder(uri) { Query = queryStrings.ToString() }.Uri.ToString();
        await this.HelperScript.ReplaceHistoryStateAsync(nextUrl);

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
