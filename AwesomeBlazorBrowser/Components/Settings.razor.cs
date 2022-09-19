using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components;

public partial class Settings
{
    private Theme _Theme;

    [Inject] public HelperScriptService HelperScript { get; init; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this._Theme = await this.HelperScript.GetCurrentThemeAsync();
            this.StateHasChanged();
        }
    }

    private async Task OnClickTheme(Theme theme)
    {
        this._Theme = theme;
        await this.HelperScript.SetCurrentThemeAsync(this._Theme);
    }
}