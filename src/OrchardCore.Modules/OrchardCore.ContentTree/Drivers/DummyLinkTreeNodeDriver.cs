using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.ViewModels;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.ContentTree.Drivers
{
    public class DummyLinkTreeNodeDriver : DisplayDriver<TreeNode, DummyLinkTreeNode>
    {
        public override IDisplayResult Display(DummyLinkTreeNode treeNode)
        {
            return Combine(
                View("DummyLinkTreeNode_Fields_Summary", treeNode).Location("Summary", "Content"),
                View("DummyLinkTreeNode_Fields_Thumbnail", treeNode).Location("Thumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(DummyLinkTreeNode treeNode)
        {
            return Initialize<DummyLinkTreeNodeViewModel>("DummyLinkTreeNode_Fields_Edit", model =>
            {
                model.LinkText = treeNode.LinkText;
                model.LinkUrl = treeNode.LinkUrl;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(DummyLinkTreeNode treeNode, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(treeNode, Prefix, x => x.LinkUrl, x => x.LinkText);
            return Edit(treeNode);
        }
    }
}
