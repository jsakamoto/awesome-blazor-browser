namespace AwesomeBlazor.Models;

public class AwesomeBlazorParserOptions
{
    public List<string> SkipCategoryNames { get; set; } = ["Contents"];

    public string EndCategoryName { get; set; } = "License";
}
