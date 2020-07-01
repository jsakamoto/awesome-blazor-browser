using System;
using System.Threading.Tasks;
using AwesomeBlazor.Models;

namespace AwesomeBlazorBrowser
{
    public partial class App
    {
        private AwesomeResourceGroup RootGroup = new AwesomeResourceGroup();

        private string Keywords = "";

        private bool ParsingComplete = false;

        private bool Loading = true;

        protected override async Task OnInitializedAsync()
        {
            var url = "https://raw.githubusercontent.com/AdrienTorris/awesome-blazor/master/README.md";
            var awesomeBlazorContents = await this.HttpClient.GetStringAsync(url);
            this.RootGroup = AwesomeBlazorParser.ParseMarkdown(awesomeBlazorContents);
            this.ParsingComplete = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ParsingComplete == true && Loading == true)
            {
                Loading = false;
                StateHasChanged();

                var uriFragment = new Uri(NavigationManager.Uri).Fragment;
                if (uriFragment != "")
                {
                    await JS.ScrollToAnchorAsync(uriFragment, smooth: false);
                }
            }
        }

        private void UpdateRootGroupVisibility()
        {
            this.RootGroup.SubGroups.UpdateVisibiltyByKeywordFilter(Keywords);
        }

        private void OnChangeGroupState()
        {
            this.UpdateRootGroupVisibility();
        }

        private void OnChangeKeywords(string keywords)
        {
            this.Keywords = keywords;
            this.UpdateRootGroupVisibility();
        }
    }
}
