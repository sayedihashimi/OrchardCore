using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.Menu.Trees
{
    /// <summary>
    /// Adds a menu for each list content type to a <see cref="ContentTreePreset"/>. 
    /// </summary>
    public class MenuTreeNode : MenuItem
    {
        //contentItemId's of the menu content items to display on the content tree
        public string[] Selected { get; set; }
    }
}
