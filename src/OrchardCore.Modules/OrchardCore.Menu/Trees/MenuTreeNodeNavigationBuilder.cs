using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Services;
using OrchardCore.ContentTree.Trees;
using OrchardCore.Environment.Navigation;
using System.Linq;

using YesSql;
using OrchardCore.ContentManagement;



namespace OrchardCore.Menu.Trees
{
    public class MenuTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public MenuTreeNodeNavigationBuilder(
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager,
            ISession session,
            IStringLocalizer<MenuTreeNodeNavigationBuilder> localizer)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _session = session;
            T = localizer;
        }

        public string Name => typeof(MenuTreeNode).Name;
        public IStringLocalizer T { get; set; }

        public void BuildNavigation(TreeNode treeNode, NavigationBuilder builder)
        {
            var tn = treeNode as MenuTreeNode;

            if (tn == null)
            {
                return;
            }

            var contentTypeDefinitions = _contentDefinitionManager.ListTypeDefinitions().OrderBy(d => d.Name);

            //var listable = contentTypeDefinitions.Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Listable).OrderBy(ctd => ctd.DisplayName);
            var selected = contentTypeDefinitions.Where(ctd => tn.Selected.ToList<string>().Contains(ctd.Name));


            builder.Add(T["Content"], "1.4", content =>
            {
                content.AddClass("content").Id("content")
               .Add(T["Menu"], "1", contentItems =>
               {
                   contentItems
                   .LinkToFirstChild(false)
                   .Action("List", "Admin", new { area = "OrchardCore.Contents" });

                   foreach (var ctd in selected)
                   {
                      
                   }
               });
            });
        }
    }
}
