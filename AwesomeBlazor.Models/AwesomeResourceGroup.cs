using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AwesomeBlazor.Models
{
    public class AwesomeResourceGroup
    {
        public Guid Id { get; } = Guid.NewGuid();

        public bool Selected { get; set; } = true;

        public bool Visible { get; set; } = true;

        public string Title { get; }

        public string TitleHtml { get; }

        public List<AwesomeResourceGroup> SubGroups { get; } = new List<AwesomeResourceGroup>();

        public List<AwesomeResource> Resources { get; } = new List<AwesomeResource>();

        public string ParagraphsHtml { get; set; } = "";

        private readonly string AnchorName;

        public AwesomeResourceGroup() : this(title: "", titleHtml: "")
        {
        }

        public AwesomeResourceGroup(string title, string titleHtml)
        {
            Title = title;
            TitleHtml = titleHtml;
            AnchorName = Regex.Replace(title, "[^a-zA-Z0-9]", "-").ToLower();
        }

        public override string ToString()
        {
            return $"{Title}";
        }

        public string GetAnchorName(string prefix = "")
        {
            if (prefix == "") return AnchorName;
            else return prefix + "-" + AnchorName;
        }
    }
}
