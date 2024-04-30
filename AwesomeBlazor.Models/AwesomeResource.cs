namespace AwesomeBlazor.Models;

public class AwesomeResource
{
    public Guid Id { get; } = Guid.NewGuid();

    public bool Visible { get; set; } = true;

    public string Title { get; }

    public string ResourceUrl { get; }

    public string GitHubStarsUrl { get; }

    public string LastCommitUrl { get; }

    public string DescriptionText { get; set; }

    public string DescriptionHtml { get; set; }

    public AwesomeResource(string title, string resourceUrl, string gitHubStarsUrl, string lastCommitUrl, string descriptionText, string descriptionHtml)
    {
        this.Title = title;
        this.ResourceUrl = resourceUrl;
        this.GitHubStarsUrl = gitHubStarsUrl;
        this.LastCommitUrl = lastCommitUrl;
        this.DescriptionText = descriptionText;
        this.DescriptionHtml = descriptionHtml;
    }

    public override string ToString()
    {
        return $"{this.Title}";
    }
}
