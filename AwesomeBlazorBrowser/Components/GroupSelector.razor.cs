﻿using AwesomeBlazor.Models;
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
        public EventCallback<AwesomeResourceGroup> OnChangeGroupState { get; set; }

        private async Task OnClickGroupLink(string anchorName)
        {
            await this.JS.ScrollToAnchorAsync(anchorName, smooth: true, changeUrl: true);
        }

        private Task OnClickToggleBox(AwesomeResourceGroup group)
        {
            group.SelectionState = group.SelectionState != SelectionState.Selected ? SelectionState.Selected : SelectionState.Unselected;
            return this.OnChangeGroupState.InvokeAsync(group);
        }

        private Task OnClickExpandBox(AwesomeResourceGroup group)
        {
            group.Expanded = !group.Expanded;
            return this.OnChangeGroupState.InvokeAsync(group);
        }

        public string GetSubGroupsHeight(AwesomeResourceGroup group)
        {
            var expandedDescendantsCount = group.GetExpandedDescendantsCount();
            return (expandedDescendantsCount * 26) + "px";
        }

        private Task _OnChangeSubGroupState(AwesomeResourceGroup group)
        {
            return this.OnChangeGroupState.InvokeAsync(group);
        }
    }
}
