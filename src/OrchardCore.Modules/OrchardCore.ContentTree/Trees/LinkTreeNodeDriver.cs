using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Trees;
using OrchardCore.ContentTree.ViewModels;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.ContentTree.Trees
{
    public class LinkTreeNodeDriver : DisplayDriver<TreeNode, LinkTreeNode>
    {
        public override IDisplayResult Display(LinkTreeNode treeNode)
        {
            return Combine(
                View("LinkTreeNode_Fields_Summary", treeNode).Location("Summary", "Content"),
                View("LinkTreeNode_Fields_Thumbnail", treeNode).Location("Thumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(LinkTreeNode treeNode)
        {
            return Initialize<LinkTreeNodeViewModel>("LinkTreeNode_Fields_Edit", model =>
            {
                model.LinkText = treeNode.LinkText;
                model.LinkUrl = treeNode.LinkUrl;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(LinkTreeNode treeNode, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(treeNode, Prefix, x => x.LinkUrl, x => x.LinkText);
            return Edit(treeNode);
        }
    }
}
