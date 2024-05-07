using System.Timers;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components;

public partial class AppBar : IDisposable
{
    private string Keywords = "";

    private readonly System.Timers.Timer DebounceTimer = new(interval: 500) { AutoReset = false };

    [Inject] public NavigationManager NavigationManager { get; init; } = null!;

    [Parameter] public bool EnableSearchBox { get; set; }

    [Parameter] public EventCallback<string> OnChangeKeywords { get; set; }

    [Parameter] public EventCallback OnClickGroupPanelMenu { get; set; }

    [Parameter] public EventCallback OnClickSettings { get; set; }

    protected override void OnInitialized()
    {
        this.DebounceTimer.Elapsed += this.DebounceTimer_Elapsed;

        var uri = new Uri(this.NavigationManager.Uri);
        var queryStrings = HttpUtility.ParseQueryString(uri.Query);
        this.Keywords = queryStrings["k"] ?? this.Keywords;
        this.OnChangeKeywords.InvokeAsync(this.Keywords);
    }

    private void OnInputKeywords()
    {
        this.DebounceTimer.Stop();
        this.DebounceTimer.Start();
    }

    private void DebounceTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        this.InvokeAsync(() => this.OnChangeKeywords.InvokeAsync(this.Keywords));
    }

    public void Dispose()
    {
        this.DebounceTimer.Elapsed -= this.DebounceTimer_Elapsed;
        this.DebounceTimer.Dispose();
    }
}
