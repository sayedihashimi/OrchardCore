using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Trees;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Services
{
    public class DummyLinkTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        public string Name => typeof(DummyLinkTreeNode).Name;

        public void BuildNavigation(TreeNode treeNode, NavigationBuilder builder)
        {
            var dl = treeNode as DummyLinkTreeNode;

            if (dl == null)
            {
                return;
            }

            builder
                .Add(new LocalizedString("Content","Content"), content => content
                    .Add(new LocalizedString(dl.LinkText, dl.LinkText), "1.5", layers => layers
                        .Permission(Permissions.ManageContentTree)
                        .Action("List", "Admin", new { area = "OrchardCore.ContentTree" })
                        .LocalNav()
                    ));

        }
    }
}
