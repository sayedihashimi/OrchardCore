using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Models
{
    public abstract class TreeNode 
    {
        public TreeNode()
        {
            TreeNodes = new List<TreeNode>();
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public List<TreeNode> TreeNodes { get; set; }
    }
}
