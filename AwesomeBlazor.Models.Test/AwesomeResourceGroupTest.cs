namespace AwesomeBlazor.Models.Test;

public class AwesomeResourceGroupTest
{
    [Test]
    public void ParentId_Test()
    {
        var root = new AwesomeResourceGroup { Id = "/" };
        var child1 = new AwesomeResourceGroup { Id = "/foo/" };
        var child2 = new AwesomeResourceGroup { Id = "/foo/bar/" };
        var child3 = new AwesomeResourceGroup { Id = "/foo/bar/fizz/" };

        root.ParentId.Is("/");
        child1.ParentId.Is("/");
        child2.ParentId.Is("/foo/");
        child3.ParentId.Is("/foo/bar/");
    }

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

    [Test]
    public void ForEachAll_Test()
    {
        // Given
        var rootGroup = new AwesomeResourceGroup
        {
            SubGroups = {
                new AwesomeResourceGroup{
                    Id = "/samples/",
                    Title = "Samples",
                    Resources = {
                        new AwesomeResource { Id = "/samples/blazing-pizza/", Title = "Blazing Pizza" },
                    } },
                new AwesomeResourceGroup {
                    Id = "/libraries/",
                    Title = "Libraries",
                    SubGroups = {
                        new AwesomeResourceGroup {
                            Id = "/libraries/maps/",
                            Title = "Maps",
                            Resources = {
                                new AwesomeResource { Id = "/libraries/maps/blazor-maps/", Title = "Blazor Maps" },
                            } },
                        new AwesomeResourceGroup{
                            Id = "/libraries/charts/",
                            Title = "Charts",
                            Resources = {
                                new AwesomeResource { Id = "/libraries/charts/blazing-line-chart/", Title = "Blazing Line Chart" },
                                new AwesomeResource { Id = "/libraries/charts/blazing-bar-chart/", Title = "Blazing Bar Chart" },
                            } },
                    } },
            }
        };


        // When
        var idAndTitles = new List<string>();
        rootGroup.ForEachAll(
            g => idAndTitles.Add($"{g.Id} | {g.Title}"),
            r => idAndTitles.Add($"{r.Id} | {r.Title}")
        );

        // Then
        idAndTitles.Order().Is(
            "/ | ",
            "/libraries/ | Libraries",
            "/libraries/charts/ | Charts",
            "/libraries/charts/blazing-bar-chart/ | Blazing Bar Chart",
            "/libraries/charts/blazing-line-chart/ | Blazing Line Chart",
            "/libraries/maps/ | Maps",
            "/libraries/maps/blazor-maps/ | Blazor Maps",
            "/samples/ | Samples",
            "/samples/blazing-pizza/ | Blazing Pizza"
        );
    }
}
