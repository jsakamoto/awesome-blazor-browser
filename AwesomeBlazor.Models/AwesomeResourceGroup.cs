using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AwesomeBlazor.Models
{
    public class AwesomeResourceGroup
    {
        public Guid Id { get; } = Guid.NewGuid();

        private bool Selected = true;

        public SelectionState SelectionState
        {
            get
            {
                if (!this.SubGroups.Any()) return Selected ? SelectionState.Selected : SelectionState.Unselected;
                if (this.SubGroups.All(g => g.SelectionState == SelectionState.Unselected)) return SelectionState.Unselected;
                if (this.SubGroups.All(g => g.SelectionState == SelectionState.Selected)) return SelectionState.Selected;
                return SelectionState.SelectedAny;
            }
            set
            {
                if (!this.SubGroups.Any()) this.Selected = value == SelectionState.Selected;
                foreach (var subGropup in SubGroups)
                {
                    subGropup.SelectionState = value;
                }
            }
        }

        public bool Visible { get; set; } = true;

        public bool Expanded { get; set; } = true;

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

        public int GetExpandedDescendantsCount()
        {
            if (this.Expanded == false) return 0;
            var childrenCount = this.SubGroups.Count;
            var descendantsCount = this.SubGroups.Select(subGrp => subGrp.GetExpandedDescendantsCount()).Sum();
            return childrenCount + descendantsCount;
        }
    }
}
