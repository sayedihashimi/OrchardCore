using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrchardCore.ContentTree.Models
{
    public class DummyLinkTreeNode : TreeNode
    {
        public DummyLinkTreeNode()
        {
            Name = "DummyLinkTreeNode";
        }

        [Required]
        public string LinkText { get; set; }
        [Required]
        public string LinkUrl { get; set; }
    }
}
