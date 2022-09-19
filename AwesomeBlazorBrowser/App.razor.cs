using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser;

public partial class App : IAsyncDisposable
{
    [Inject] HttpClient HttpClient { get; init; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    [Inject] public IServiceProvider ServiceProvider { get; init; } = null!;

    private HelperScriptService? _HelperScript;

    private AwesomeResourceGroup RootGroup = new();

    private string Keywords = "";

    private bool Loading = true;

    private bool GroupPanelExpanded = false;

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
        if (this.ParsingCompletionSource.Task.IsCompleted == true && this.Loading == true)
        {
            this.Loading = false;
            this.StateHasChanged();

            this._HelperScript = this.ServiceProvider.GetRequiredService<HelperScriptService>();
            var uriFragment = new Uri(this.NavigationManager.Uri).Fragment;
            if (uriFragment != "")
            {
                await this._HelperScript.ScrollToAnchorAsync(uriFragment, smooth: false);
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

        var nextUrl = this.NavigationManager.GetUriWithQueryParameter("k", keywords);
        var uriFragment = new Uri(this.NavigationManager.Uri).Fragment;
        if (uriFragment != "") nextUrl += uriFragment;
        this.NavigationManager.NavigateTo(nextUrl, new NavigationOptions() { ReplaceHistoryEntry = true });
    }

    private void OnClickGroupPanelMenu()
    {
        this.GroupPanelExpanded = !this.GroupPanelExpanded;
    }

    private void OnClickGroupLink()
    {
        this.GroupPanelExpanded = false;
    }

    public async ValueTask DisposeAsync()
    {
        if (this._HelperScript != null) await this._HelperScript.DisposeAsync();
    }
}
