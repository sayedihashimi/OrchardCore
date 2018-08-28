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

namespace OrchardCore.Contents.Trees
{
    public class AllContentTypesTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public AllContentTypesTreeNodeNavigationBuilder(
            IContentDefinitionManager contentDefinitionManager,
            IStringLocalizer<AllContentTypesTreeNodeNavigationBuilder> localizer)
        {
            _contentDefinitionManager = contentDefinitionManager;
            T = localizer;
        }

        public string Name => typeof(AllContentTypesTreeNode).Name;
        public IStringLocalizer T { get; set; }

        public void BuildNavigation(TreeNode treeNode, NavigationBuilder builder)
        {
            var tn = treeNode as AllContentTypesTreeNode;

            if (tn == null)
            {
                return;
            }

            var contentTypeDefinitions = _contentDefinitionManager.ListTypeDefinitions().OrderBy(d => d.Name);

            var creatable = contentTypeDefinitions.Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Creatable).OrderBy(ctd => ctd.DisplayName);
            var listable = contentTypeDefinitions.Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Listable).OrderBy(ctd => ctd.DisplayName);


            builder.Add(T["Content"], "1.4", content =>
            {
                content.AddClass("content").Id("content")
               .Add(T["Content By Type"], "1", contentItems =>
               {
                   contentItems
                   .LinkToFirstChild(false)
                   .Permission(Permissions.EditOwnContent)
                   .Action("List", "Admin", new { area = "OrchardCore.Contents" });

                   foreach (var ctd in listable)
                   {
                       var rv = new RouteValueDictionary();
                       // todo: merge filterbox branch or this won't work yet because the content item list is not ready to read the querystring.
                       rv.Add("contentType", ctd.Name);
                       contentItems.Add(new LocalizedString(ctd.DisplayName, ctd.DisplayName), t => t.Action("List", "Admin", "OrchardCore.Contents", rv));
                   }
               });
            });

            //builder
            //    .Add(new LocalizedString("Content","Content"), content => content
            //        .Add(new LocalizedString(tn.LinkText, tn.LinkText), "1.5", layers => layers
            //            .Permission(Permissions.EditOwnContent)
            //            .Action("List", "Admin", new { area = "OrchardCore.ContentTree" })
            //            .LocalNav()
            //        ));

        }
    }
}
