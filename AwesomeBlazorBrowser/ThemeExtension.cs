namespace AwesomeBlazorBrowser;

public static class ThemeExtension
{
    public static Theme Parse(string themeString)
    {
        return themeString.ToLower() switch
        {
            "theme-system-default" => Theme.SystemDefault,
            "theme-light-mode" => Theme.LightMode,
            "theme-dark-mode" => Theme.DarkMode,
            _ => Theme.SystemDefault
        };
    }

    public static string ToKebabCase(this Theme theme)
    {
        return theme switch
        {
            Theme.SystemDefault => "theme-system-default",
            Theme.LightMode => "theme-light-mode",
            Theme.DarkMode => "theme-dark-mode",
            _ => "theme-system-default"
        };
    }
}
