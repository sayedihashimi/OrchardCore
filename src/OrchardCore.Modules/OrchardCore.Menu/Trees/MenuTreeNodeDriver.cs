using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Trees;
using OrchardCore.ContentTree.ViewModels;
using System.Linq;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using YesSql;
using OrchardCore.Menu.Models;
using OrchardCore.ContentManagement.Records;

namespace OrchardCore.Menu.Trees
{
    public class MenuTreeNodeDriver : DisplayDriver<TreeNode, MenuTreeNode>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public MenuTreeNodeDriver(
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager,
            ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _session = session;
        }
        public override IDisplayResult Display(MenuTreeNode treeNode)
        {
            return Combine(
                View("MenuTreeNode_Fields_Summary", treeNode).Location("Summary", "Content"),
                View("MenuTreeNode_Fields_Thumbnail", treeNode).Location("Thumbnail", "Content")
            );
        }

        public override IDisplayResult Edit(MenuTreeNode treeNode)
        {
            // display all contentitems that are of any type that has a menu part.
            // todo: look for a more efficient way to do this.

            var menuTypes = _contentDefinitionManager.ListTypeDefinitions()
                .Where(x => x.Parts.Any(p => p.PartDefinition.Name == typeof(MenuPart).Name))
                .ToList();

            
            var temp = new List<ContentItem>();
            foreach (var menutype in menuTypes)
            {
                var menus = _session.Query<ContentItem, ContentItemIndex>()
                    .With<ContentItemIndex>(x => x.Latest)
                    .With<ContentItemIndex>(x => x.ContentType == menutype.Name)
                    .ListAsync()
                    .Result;
                temp.AddRange(menus);
            }

            var menuItems = temp.ToDictionary(
                x => x.ContentItemId,
                x => _contentManager.PopulateAspectAsync<ContentItemMetadata>(x).Result.DisplayText);

            return Initialize<MenuTreeNodeViewModel>("MenuTreeNode_Fields_Edit", model =>
            {
                model.AvailableMenuContentItems = menuItems;
                model.Selected = treeNode.Selected;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(MenuTreeNode treeNode, IUpdateModel updater)
        {
            // Initializes the value to empty otherwise the model is not updated if no type is selected.
            treeNode.Selected= Array.Empty<string>();

            await updater.TryUpdateModelAsync(treeNode, Prefix, x => x.Selected);
            return Edit(treeNode);
        }
    }
}
