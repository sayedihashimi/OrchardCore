using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Trees
{
    public class LinkTreeNode : TreeNode
    {
        [Required]
        public string LinkText { get; set; }
        [Required]
        public string LinkUrl { get; set; }
    }

}
