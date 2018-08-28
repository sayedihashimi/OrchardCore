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

namespace OrchardCore.Contents.Trees
{
    public class AllContentTypesTreeNodeDriver : DisplayDriver<TreeNode, AllContentTypesTreeNode>
    {
        public override IDisplayResult Display(AllContentTypesTreeNode treeNode)
        {
            return Combine(
                View("AllContentTypesTreeNode_Fields_Summary", treeNode).Location("Summary", "Content"),
                View("AllContentTypesTreeNode_Fields_Thumbnail", treeNode).Location("Thumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(AllContentTypesTreeNode treeNode)
        {
            return Initialize<AllContentTypesTreeNodeViewModel>("AllContentTypesTreeNode_Fields_Edit", model =>
            {
                model.Name = treeNode.Name;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(AllContentTypesTreeNode treeNode, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(treeNode, Prefix, x => x.Name);
            return Edit(treeNode);
        }
    }
}
