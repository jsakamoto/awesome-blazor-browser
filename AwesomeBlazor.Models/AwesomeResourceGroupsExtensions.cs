namespace AwesomeBlazor.Models;

public static class AwesomeResourceGroupsExtensions
{
    public static IEnumerable<AwesomeResourceGroup> EnumGroupsDescendants(this IEnumerable<AwesomeResourceGroup> groups)
    {
        foreach (var group in groups)
        {
            yield return group;
            foreach (var subGroup in group.SubGroups.EnumGroupsDescendants())
            {
                yield return subGroup;
            }
        }
    }
}
