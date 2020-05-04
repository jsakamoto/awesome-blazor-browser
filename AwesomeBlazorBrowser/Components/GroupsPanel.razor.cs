using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components
{
    public partial class GroupsPanel
    {
        [Parameter]
        public AwesomeResourceGroup Group { get; set; } = new AwesomeResourceGroup();

        [Parameter]
        public EventCallback OnChangeGroupSelection { get; set; }

        private Task _OnChangeGroupSelection(AwesomeResourceGroup group)
        {
            return this.OnChangeGroupSelection.InvokeAsync(group);
        }

        private Task OnClickToggleBox(AwesomeResourceGroup group)
        {
            group.SelectionState = group.SelectionState != SelectionState.Selected ? SelectionState.Selected : SelectionState.Unselected;
            return this.OnChangeGroupSelection.InvokeAsync(group);
        }

        private void OnClickSelectUnselectAll()
        {
            //var nextState = this.Groups.EnumGroupsDescendants().Any(g => g.SelectionState == SelectionState.Unselected) ? SelectionState.Selected : SelectionState.Unselected;
            //foreach (var group in this.Groups.EnumGroupsDescendants())
            //{
            //    group.SelectionState = nextState;
            //}
            //this.OnChangeGroupSelection.InvokeAsync(null);
        }
    }
}
