using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Services;
using OrchardCore.ContentTree.Trees;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Trees
{
    public class LinkTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        public string Name => typeof(LinkTreeNode).Name;

        public void BuildNavigation(TreeNode treeNode, NavigationBuilder builder)
        {
            var ltn = treeNode as LinkTreeNode;

            if (ltn == null)
            {
                return;
            }
            
            builder
                .Add(new LocalizedString("Content","Content"), content => content
                    .Add(new LocalizedString(ltn.LinkText, ltn.LinkText), "1.5", layers => layers
                        .Permission(Permissions.ManageContentTree)
                        .Url(ltn.LinkUrl)                        
                        .LocalNav()
                    ));

        }
    }
}
