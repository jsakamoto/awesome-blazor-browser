using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AwesomeBlazor.Models;

public class AwesomeResourceGroup
{
    [JsonIgnore]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string? _parentId;

    [JsonIgnore]
    public string ParentId => this._parentId ??= this.Id == "/" ? "/" : string.Join('/', this.Id.TrimEnd('/').Split('/')[0..^1]) + '/';

    private bool Selected = true;

    [JsonIgnore]
    public SelectionState SelectionState
    {
        get
        {
            if (!this.SubGroups.Any()) return this.Selected ? SelectionState.Selected : SelectionState.Unselected;
            if (this.SubGroups.All(g => g.SelectionState == SelectionState.Unselected)) return SelectionState.Unselected;
            if (this.SubGroups.All(g => g.SelectionState == SelectionState.Selected)) return SelectionState.Selected;
            return SelectionState.SelectedAny;
        }
        set
        {
            if (!this.SubGroups.Any()) this.Selected = value == SelectionState.Selected;
            foreach (var subGropup in this.SubGroups)
            {
                subGropup.SelectionState = value;
            }
        }
    }

    [JsonIgnore]
    public bool Visible { get; set; } = true;

    [JsonIgnore]
    public bool Expanded { get; set; } = true;

    public string Title { get; init; } = "";

    public string TitleHtml { get; init; } = "";

    [JsonIgnore]
    public List<AwesomeResourceGroup> SubGroups { get; } = [];

    [JsonIgnore]
    public List<AwesomeResource> Resources { get; } = [];

    public string ParagraphsHtml { get; set; } = "";

    [JsonIgnore]
    public byte[] Embedding { get; set; } = [];

    private string? _anchorName;

    [JsonIgnore]
    public string AnchorName => this._anchorName ??= Regex.Replace(this.Title, "[^a-zA-Z0-9]", "-").ToLower();

    public override string ToString()
    {
        return $"{this.Title}";
    }

    public string GetAnchorName(string prefix = "")
    {
        if (prefix == "") return this.AnchorName;
        else return prefix + "-" + this.AnchorName;
    }

    public int GetExpandedDescendantsCount()
    {
        if (this.Expanded == false) return 0;
        var childrenCount = this.SubGroups.Count;
        var descendantsCount = this.SubGroups.Select(subGrp => subGrp.GetExpandedDescendantsCount()).Sum();
        return childrenCount + descendantsCount;
    }
}
