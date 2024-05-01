using AwesomeBlazor.Models;

namespace AwesomeBlazor.Store;

public class AwesomeBlazorStore(
    HttpClient httpClient
)
{
    private const string _awesomeBlazorUrl = "https://raw.githubusercontent.com/AdrienTorris/awesome-blazor/master/README.md";

    public async ValueTask<AwesomeResourceGroup> GetAwesomeBlazorContentAsync()
    {
        var awesomeBlazorContents = await httpClient.GetStringAsync(_awesomeBlazorUrl);
        var rootGroup = AwesomeBlazorParser.ParseMarkdown(awesomeBlazorContents);

        return rootGroup;
    }
}
