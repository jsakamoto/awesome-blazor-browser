using System.Collections.Generic;

namespace AwesomeBlazor.Models
{
    public class AwesomeBlazorParserOptions
    {
        public List<string> SkipCategoryNames { get; set; } = new List<string>() { "Contents" };

        public string EndCategoryName { get; set; } = "License";
    }
}
