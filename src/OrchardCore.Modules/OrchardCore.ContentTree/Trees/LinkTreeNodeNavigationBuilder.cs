using System;
using System.Collections.Generic;
using System.Linq;
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

        public void BuildNavigation(MenuItem menuItem, NavigationBuilder builder, IEnumerable<ITreeNodeNavigationBuilder> treeNodeBuilders)
        {
            var ltn = menuItem as LinkTreeNode;

            if ((ltn == null) ||( !ltn.Enabled))
            {
                return;
            }

            builder.Add(new LocalizedString(ltn.LinkText, ltn.LinkText), itemBuilder => {

                // Add the actual link
                itemBuilder.Url(ltn.LinkUrl);
                ltn.CustomClasses.ToList().ForEach( x => itemBuilder.AddClass(x));

                // Add the other ITreeNodeNavigationBuilder build themselves as children of this link
                foreach (var childTreeNode in menuItem.Items)
                {
                    var treeBuilder = treeNodeBuilders.Where(x => x.Name == childTreeNode.GetType().Name).FirstOrDefault();
                    treeBuilder.BuildNavigation(childTreeNode, itemBuilder, treeNodeBuilders);
                }
            });
        }
    }
}
