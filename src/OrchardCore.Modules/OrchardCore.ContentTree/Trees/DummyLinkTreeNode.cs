using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Trees
{
    public class DummyLinkTreeNode : TreeNode
    {
        [Required]
        public string LinkText { get; set; }
        [Required]
        public string LinkUrl { get; set; }

        //public void BuildNavigation(string name, NavigationBuilder builder)
        //{
        //    if (!String.Equals(name, "contenttree", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return;
        //    }

        //    builder
        //        .Add(S["Content"], content => content
        //            .Add(new LocalizedString(this.LinkText, this.LinkText), "1.5", layers => layers
        //                .Permission(Permissions.ManageContentTree)
        //                .Action("List", "Admin", new { area = "OrchardCore.ContentTree" })
        //                .LocalNav()
        //            ));
        //}
    }
}
