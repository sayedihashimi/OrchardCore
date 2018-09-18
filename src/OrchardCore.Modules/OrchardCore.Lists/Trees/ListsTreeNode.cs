using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.Lists.Trees
{
    /// <summary>
    /// Adds a menu for each list content type to a <see cref="ContentTreePreset"/>. 
    /// </summary>
    public class ListsTreeNode : MenuItem
    {
        public string[] ContentTypes { get; set; }

        public bool Enabled { get; set; } = true;

        // classes added through the admin.
        public string[] CustomClasses { get; set; } = Array.Empty<string>();

        public bool AddContentTypeAsParent { get; set; } = true;

    }
}
