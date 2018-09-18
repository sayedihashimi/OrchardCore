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
    public class LinkTreeNode : MenuItem
    {
        [Required]
        public string LinkText { get; set; }

        public string LinkUrl { get; set; }

        public bool Enabled { get; set; } = true;

        // classes added through the admin.
        public string[] CustomClasses { get; set; } = Array.Empty<string>();
    }

}
