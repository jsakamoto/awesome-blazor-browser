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
        var helpder = await this.GetHelperAsync();
        await helpder.InvokeVoidAsync("installHashWatcher");
    }

    public async ValueTask ScrollToAnchorAsync(string anchorName, bool smooth, bool changeUrl = false)
    {
        var helpder = await this.GetHelperAsync();
        await helpder.InvokeVoidAsync("scrollToAnchor", anchorName.TrimStart('#'), smooth, changeUrl);
    }

    public async ValueTask<Theme> GetCurrentThemeAsync()
    {
        var helpder = await this.GetHelperAsync();
        var themeStr = await helpder.InvokeAsync<string>("getCurrentTheme");
        return ThemeExtension.Parse(themeStr);
    }

    public async ValueTask SetCurrentThemeAsync(Theme theme)
    {
        var helpder = await this.GetHelperAsync();
        await helpder.InvokeVoidAsync("setCurrentTheme", theme.ToKebabCase());
    }

    public async ValueTask DisposeAsync()
    {
        var helpder = await this.GetHelperAsync();
        await helpder.DisposeAsync();
    }
}
