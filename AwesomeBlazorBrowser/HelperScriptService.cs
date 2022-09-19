using Microsoft.JSInterop;

namespace AwesomeBlazorBrowser;

public class HelperScriptService : IAsyncDisposable
{
    private readonly Task<IJSObjectReference> _HelperLoader;

    public HelperScriptService(IJSRuntime jsRuntime)
    {
        this._HelperLoader = jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/helper.js").AsTask();
    }

    public async ValueTask ScrollToAnchorAsync(string anchorName, bool smooth, bool changeUrl = false)
    {
        var helpder = await _HelperLoader;
        await helpder.InvokeVoidAsync("scrollToAnchor", anchorName.TrimStart('#'), smooth, changeUrl);
    }

    public async ValueTask DisposeAsync()
    {
        var helpder = await _HelperLoader;
        await helpder.DisposeAsync();
    }
}
