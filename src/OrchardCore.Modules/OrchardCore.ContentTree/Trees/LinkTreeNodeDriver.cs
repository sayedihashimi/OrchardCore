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

namespace OrchardCore.ContentTree.Trees
{
    public class LinkTreeNodeDriver : DisplayDriver<MenuItem, LinkTreeNode>
    {
        public override IDisplayResult Display(LinkTreeNode treeNode)
        {
            return Combine(
                View("LinkTreeNode_Fields_TreeSummary", treeNode).Location("TreeSummary", "Content"),
                View("LinkTreeNode_Fields_TreeThumbnail", treeNode).Location("TreeThumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(LinkTreeNode treeNode)
        {
            return Initialize<LinkTreeNodeViewModel>("LinkTreeNode_Fields_TreeEdit", model =>
            {
                model.LinkText = treeNode.LinkText;
                model.LinkUrl = treeNode.LinkUrl;
                model.Enabled = treeNode.Enabled;
                model.CustomClasses = string.Join(",", treeNode.CustomClasses);
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(LinkTreeNode treeNode, IUpdateModel updater)
        {
            var model = new LinkTreeNodeViewModel();
            if(await updater.TryUpdateModelAsync(model, Prefix, x => x.LinkUrl, x => x.LinkText, x => x.Enabled, x => x.CustomClasses))
            {
                treeNode.LinkText = model.LinkText;
                treeNode.LinkUrl = model.LinkUrl;
                treeNode.Enabled = model.Enabled;
                treeNode.CustomClasses = string.IsNullOrEmpty(model.CustomClasses) ? Array.Empty<string>() : model.CustomClasses.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            };
            
            return Edit(treeNode);
        }
    }
}
