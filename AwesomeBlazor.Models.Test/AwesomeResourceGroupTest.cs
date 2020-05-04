using AwesomeBlazor.Models;
using AwesomeBlazor.Models.Test;
using Xunit;

namespace AwesomeBlazor.Test
{
    public class AwesomeResourceGroupTest
    {
        [Fact]
        public void SelectionState_Test()
        {
            var contents = TestFixture.GetContentsForTest();
            var root = AwesomeBlazorParser.ParseMarkdown(contents);

            var parentGroup = root.SubGroups[4];
            var childGroup1 = parentGroup.SubGroups[0];
            var childGroup2 = parentGroup.SubGroups[1];

            childGroup1.SelectionState = SelectionState.Unselected;
            parentGroup.SelectionState.Is(SelectionState.SelectedAny);

            parentGroup.SelectionState = SelectionState.Selected;
            childGroup1.SelectionState.Is(SelectionState.Selected);
            childGroup2.SelectionState.Is(SelectionState.Selected);

            childGroup1.SelectionState = SelectionState.Unselected;
            childGroup2.SelectionState = SelectionState.Unselected;
            parentGroup.SelectionState.Is(SelectionState.Unselected);

            childGroup1.SelectionState = SelectionState.Selected;
            childGroup2.SelectionState = SelectionState.Selected;
            parentGroup.SelectionState.Is(SelectionState.Selected);

            parentGroup.SelectionState = SelectionState.Unselected;
            childGroup1.SelectionState.Is(SelectionState.Unselected);
            childGroup2.SelectionState.Is(SelectionState.Unselected);
        }
    }
}
