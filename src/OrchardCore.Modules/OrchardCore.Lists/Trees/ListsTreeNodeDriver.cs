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
using OrchardCore.Environment.Navigation;

namespace OrchardCore.Lists.Trees
{
    public class ListsTreeNodeDriver : DisplayDriver<MenuItem, ListsTreeNode>
    {
        public override IDisplayResult Display(ListsTreeNode treeNode)
        {
            return Combine(
                View("ListsTreeNode_Fields_TreeSummary", treeNode).Location("TreeSummary", "Content"),
                View("ListsTreeNode_Fields_TreeThumbnail", treeNode).Location("TreeThumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(ListsTreeNode treeNode)
        {
            return Initialize<ListsTreeNodeViewModel>("ListsTreeNode_Fields_TreeEdit", model =>
            {
                model.ContentTypes = treeNode.ContentTypes;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ListsTreeNode treeNode, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(treeNode, Prefix, x => x.ContentTypes);
            return Edit(treeNode);
        }
    }
}
