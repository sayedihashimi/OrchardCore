using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.Contents.Trees
{
    /// <summary>
    /// Adds a menu for each selected content type to a <see cref="ContentTreePreset"/>. 
    /// </summary>
    public class ContentTypesTreeNode : MenuItem
    {
        public bool ShowAll { get; set; }
        public string[] ContentTypes { get; set; }
    }
}
