using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

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

        private Task OnClickExpandBox(AwesomeResourceGroup group)
        {
            group.Expanded = !group.Expanded;
            return Task.CompletedTask;
        }

        public string GetSubGroupsHeight(AwesomeResourceGroup group)
        {
            return group.Expanded ? (group.SubGroups.Count * 26) + "px" : "0";
        }

        private Task _OnChangeSubGroupSelection(AwesomeResourceGroup group)
        {
            return this.OnChangeGroupSelection.InvokeAsync(group);
        }
    }
}
