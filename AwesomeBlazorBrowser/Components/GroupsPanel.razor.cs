using AwesomeBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components;

public partial class GroupsPanel
{
    [Parameter]
    public AwesomeResourceGroup Group { get; set; } = new AwesomeResourceGroup();

    [Parameter]
    public EventCallback OnChangeGroupState { get; set; }

    [Parameter]
    public EventCallback OnClickGroupLink { get; set; }

    private Task _OnChangeGroupState(AwesomeResourceGroup group)
    {
        return this.OnChangeGroupState.InvokeAsync(group);
    }

    private Task OnClickToggleBox(AwesomeResourceGroup group)
    {
        group.SelectionState = group.SelectionState switch
        {
            SelectionState.Selected => SelectionState.Unselected,
            SelectionState.Unselected => SelectionState.Selected,
            _ => SelectionState.Selected
        };

        return this.OnChangeGroupState.InvokeAsync(group);
    }
}
