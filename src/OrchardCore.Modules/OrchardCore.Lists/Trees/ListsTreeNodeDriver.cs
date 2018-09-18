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
                model.Enabled = treeNode.Enabled;
                model.CustomClasses = string.Join(",", treeNode.CustomClasses);
                model.AddContentTypeAsParent = treeNode.AddContentTypeAsParent;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ListsTreeNode treeNode, IUpdateModel updater)
        {
            var model = new ListsTreeNodeViewModel();

            if (await updater.TryUpdateModelAsync(model, Prefix, x => x.ContentTypes, x => x.Enabled, x => x.CustomClasses, x => x.AddContentTypeAsParent)) {
                treeNode.Enabled = model.Enabled;
                treeNode.ContentTypes = model.ContentTypes;
                treeNode.AddContentTypeAsParent = model.AddContentTypeAsParent;
                treeNode.CustomClasses =  string.IsNullOrEmpty( model.CustomClasses) ?  Array.Empty<string>() : model.CustomClasses.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            };

            return Edit(treeNode);
        }
    }
}
