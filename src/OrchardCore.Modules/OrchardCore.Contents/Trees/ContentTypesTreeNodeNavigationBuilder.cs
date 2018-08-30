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
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata.Models;

namespace OrchardCore.Contents.Trees
{
    public class ContentTypesTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ContentTypesTreeNodeNavigationBuilder(
            IContentDefinitionManager contentDefinitionManager,
            IStringLocalizer<ContentTypesTreeNodeNavigationBuilder> localizer)
        {
            _contentDefinitionManager = contentDefinitionManager;
            T = localizer;
        }

        public string Name => typeof(ContentTypesTreeNode).Name;
        public IStringLocalizer T { get; set; }

        public void BuildNavigation(TreeNode treeNode, NavigationBuilder builder)
        {
            var tn = treeNode as ContentTypesTreeNode;

            if (tn == null)
            {
                return;
            }

            var contentTypeDefinitions = _contentDefinitionManager.ListTypeDefinitions().OrderBy(d => d.Name);


            var typesToShow = GetContentTypes(tn);

            builder.Add(T["Content"], "1.4", content =>
            {
                content.AddClass("content").Id("content")
               .Add(T["Content by Type"], "1", contentItems =>
               {
                   contentItems
                   .LinkToFirstChild(false)
                   .Permission(Permissions.EditOwnContent)
                   .Action("List", "Admin", new { area = "OrchardCore.Contents" });

                   foreach (var ctd in typesToShow)
                   {
                       var rv = new RouteValueDictionary();
                       rv.Add("Options.TypeName", ctd.Name);
                       contentItems.Add(new LocalizedString(ctd.DisplayName, ctd.DisplayName), t => t.Action("List", "Admin", "OrchardCore.Contents", rv));
                   }
               });
            });
        }


        private IEnumerable<ContentTypeDefinition> GetContentTypes(ContentTypesTreeNode tn)
        {
            var typesToShow = _contentDefinitionManager.ListTypeDefinitions().
                Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Listable);

            if(tn.ShowAll == false)
            {
                typesToShow = typesToShow.Where(ctd => tn.ContentTypes.ToList<string>().Contains(ctd.Name));
            }
            
            return typesToShow.OrderBy( t => t.Name);
        }
    }
}
