using System.Text.Json.Serialization;

namespace AwesomeBlazor.Models;

public class AwesomeResource
{
    [JsonIgnore]
    public string Id { get; init; } = "/";

    public string? _parentId;

    [JsonIgnore]
    public string ParentId => this._parentId ??= this.Id == "/" ? "/" : string.Join('/', this.Id.TrimEnd('/').Split('/')[0..^1]) + '/';

    public int Order { get; init; }

    [JsonIgnore]
    public bool Visible { get; set; } = true;

    public string Title { get; init; } = "";

    public string ResourceUrl { get; init; } = "";

    public string GitHubStarsUrl { get; init; } = "";

    public string LastCommitUrl { get; init; } = "";

    public string DescriptionText { get; set; } = "";

    public string DescriptionHtml { get; set; } = "";

    [JsonIgnore]
    public byte[]? Embedding { get; set; }

    public override string ToString() => $"{this.Title}";
}
