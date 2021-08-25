using Microsoft.JSInterop;

namespace AwesomeBlazorBrowser
{
    public static class Helper
    {
        public static ValueTask ScrollToAnchorAsync(this IJSRuntime js, string anchorName, bool smooth, bool changeUrl = false)
        {
            return js.InvokeVoidAsync("AwesomeBlazorBrowser.scrollToAnchor", anchorName.TrimStart('#'), smooth, changeUrl);
        }
    }
}
