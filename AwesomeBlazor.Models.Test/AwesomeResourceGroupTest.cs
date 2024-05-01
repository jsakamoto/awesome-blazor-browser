namespace AwesomeBlazor.Models.Test;

public class AwesomeResourceGroupTest
{
    [Test]
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

    [Test]
    public void GetExpandedDescendantsCount_Test()
    {
        var root = new AwesomeResourceGroup
        {
            Expanded = true,
            SubGroups = {
                    new AwesomeResourceGroup { }, // 1
                    new AwesomeResourceGroup {    // 2
                        Expanded = true,
                        SubGroups = {
                            new AwesomeResourceGroup { }, // 3
                            new AwesomeResourceGroup { }  // 4
                        }
                    },
                    new AwesomeResourceGroup { }, // 5
                    new AwesomeResourceGroup {    // 6
                        Expanded = false,
                        SubGroups = {
                            new AwesomeResourceGroup { }, // No Count due to collapsed
                            new AwesomeResourceGroup { }
                        }
                    }
                }
        };
        root.GetExpandedDescendantsCount().Is(6);
    }
}
