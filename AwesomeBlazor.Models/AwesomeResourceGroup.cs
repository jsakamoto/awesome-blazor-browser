﻿using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AwesomeBlazor.Models;

public class AwesomeResourceGroup : IEmbeddingSource
{
    [JsonIgnore]
    public string Id { get; init; } = "/";

    public string? _parentId;

    [JsonIgnore]
    public string ParentId => this._parentId ??= this.Id == "/" ? "/" : string.Join('/', this.Id.TrimEnd('/').Split('/')[0..^1]) + '/';

    public int Order { get; init; }

    private bool _selected = true;

    [JsonIgnore]
    public SelectionState SelectionState
    {
        get
        {
            if (!this.SubGroups.Any()) return this._selected ? SelectionState.Selected : SelectionState.Unselected;
            if (this.SubGroups.All(g => g.SelectionState == SelectionState.Unselected)) return SelectionState.Unselected;
            if (this.SubGroups.All(g => g.SelectionState == SelectionState.Selected)) return SelectionState.Selected;
            return SelectionState.SelectedAny;
        }
        set
        {
            if (!this.SubGroups.Any()) this._selected = value == SelectionState.Selected;
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
    public byte[]? Embedding { get; set; }

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

    public void ReorderChildren()
    {
        foreach (var subGroup in this.SubGroups) subGroup.ReorderChildren();

        if (this.SubGroups.Count >= 2)
        {
            this.SubGroups.Sort((a, b) => a.Order - b.Order);
        }

        if (this.Resources.Count >= 2)
        {
            this.Resources.Sort((a, b) => a.Order - b.Order);
        }
    }

    public void ForEachAll(Action<AwesomeResourceGroup> actionForGroup, Action<AwesomeResource> actionForResource)
    {
        ForEachAll(this, actionForGroup, actionForResource);
    }

    private static void ForEachAll(AwesomeResourceGroup resourceGroup, Action<AwesomeResourceGroup> actionForGroup, Action<AwesomeResource> actionForResource)
    {
        actionForGroup(resourceGroup);
        foreach (var resource in resourceGroup.Resources)
        {
            actionForResource(resource);
        }
        foreach (var subGroup in resourceGroup.SubGroups)
        {
            ForEachAll(subGroup, actionForGroup, actionForResource);
        }
    }
}
