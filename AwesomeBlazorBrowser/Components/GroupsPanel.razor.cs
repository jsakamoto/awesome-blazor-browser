using System;
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
        public IEnumerable<AwesomeResourceGroup> Groups { get; set; } = Enumerable.Empty<AwesomeResourceGroup>();

        [Parameter]
        public EventCallback OnChangeGroupSelection { get; set; }

        private Task _OnChangeGroupSelection(AwesomeResourceGroup group)
        {
            return this.OnChangeGroupSelection.InvokeAsync(group);
        }

        private void OnClickSelectUnselectAll()
        {
            var nextState = this.Groups.EnumGroupsDescendants().Any(g => !g.Selected);
            foreach (var group in this.Groups.EnumGroupsDescendants())
            {
                group.Selected = nextState;
            }
            this.OnChangeGroupSelection.InvokeAsync(null);
        }
    }
}
