using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AwesomeBlazorBrowser.Components
{
    public partial class GroupSelector
    {
        [Parameter]
        public string ParentGroupAnchor { get; set; } = "";

        [Parameter]
        public IEnumerable<AwesomeResourceGroup> Groups { get; set; } = Enumerable.Empty<AwesomeResourceGroup>();

        [Parameter]
        public EventCallback<AwesomeResourceGroup> OnChangeGroupSelection { get; set; }

        private async Task OnClickGroupLink(string anchorName)
        {
            await JS.ScrollToAnchorAsync(anchorName, smooth: true, changeUrl: true);
        }

        private Task OnClickToggleBox(AwesomeResourceGroup group)
        {
            group.SelectionState = group.SelectionState != SelectionState.Selected ? SelectionState.Selected : SelectionState.Unselected;
            return this.OnChangeGroupSelection.InvokeAsync(group);
        }

        private Task _OnChangeSubGroupSelection(AwesomeResourceGroup group)
        {
            return this.OnChangeGroupSelection.InvokeAsync(group);
        }
    }
}
