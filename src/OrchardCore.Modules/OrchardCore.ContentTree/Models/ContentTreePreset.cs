using System;
using System.Collections.Generic;
using System.Text;

namespace OrchardCore.ContentTree.Models
{
    //todo: move to a abstraction project?
    public class ContentTreePreset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TreeNode> TreeNodes { get; } = new List<TreeNode>();
    }
}
