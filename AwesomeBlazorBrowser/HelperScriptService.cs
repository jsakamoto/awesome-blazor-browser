using Microsoft.JSInterop;

namespace AwesomeBlazorBrowser;

public class HelperScriptService : IAsyncDisposable
{
    private readonly IJSRuntime _JsRuntime;

    private IJSObjectReference? _Helper;

    public HelperScriptService(IJSRuntime jsRuntime)
    {
        this._JsRuntime = jsRuntime;
    }

    private async ValueTask<IJSObjectReference> GetHelperAsync()
    {
        this._Helper ??= await this._JsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/helper.js");
        return this._Helper;
    }

    public async ValueTask InstallHashWatcherAsync()
    {
        var helper = await this.GetHelperAsync();
        await helper.InvokeVoidAsync("installHashWatcher");
    }

    public async ValueTask ScrollToAnchorAsync(string anchorName, bool smooth, bool changeUrl = false)
    {
        var helper = await this.GetHelperAsync();
        await helper.InvokeVoidAsync("scrollToAnchor", anchorName.TrimStart('#'), smooth, changeUrl);
    }

    public async ValueTask<Theme> GetCurrentThemeAsync()
    {
        var helper = await this.GetHelperAsync();
        var themeStr = await helper.InvokeAsync<string>("getCurrentTheme");
        return ThemeExtension.Parse(themeStr);
    }

    public async ValueTask SetCurrentThemeAsync(Theme theme)
    {
        var helper = await this.GetHelperAsync();
        await helper.InvokeVoidAsync("setCurrentTheme", theme.ToKebabCase());
    }

    public async ValueTask DisposeAsync()
    {
        if (this._Helper is not null)
        {
            try { await this._Helper.DisposeAsync(); }
            catch (JSDisconnectedException) { }
            finally { this._Helper = null; }
        }
    }
}
