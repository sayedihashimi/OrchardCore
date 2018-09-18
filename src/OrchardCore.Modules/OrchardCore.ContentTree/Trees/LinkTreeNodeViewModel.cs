using System.Collections.Generic;

namespace OrchardCore.ContentTree.Trees
{
    public class LinkTreeNodeViewModel
    {
        public string LinkText { get; set; }

        public string LinkUrl { get; set; }

        public bool Enabled { get; set; }

        public string CustomClasses { get; set; }
    }
}
