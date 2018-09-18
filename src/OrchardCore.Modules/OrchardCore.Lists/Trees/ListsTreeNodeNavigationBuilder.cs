using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using OrchardCore.ContentTree.Services;
using OrchardCore.Environment.Navigation;
using YesSql;

namespace OrchardCore.Lists.Trees
{
    public class ListsTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public ListsTreeNodeNavigationBuilder(
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager,
            ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _session = session;
        }

        public string Name => typeof(ListsTreeNode).Name;

        public void BuildNavigation(MenuItem menuItem, NavigationBuilder builder, IEnumerable<ITreeNodeNavigationBuilder> treeNodeBuilders)
        {
            var tn = menuItem as ListsTreeNode;

            if ((tn == null) || (!tn.Enabled))
            {
                return;
            }

            var contentTypeDefinitions = _contentDefinitionManager.ListTypeDefinitions().OrderBy(d => d.Name);
                        
            var selected = contentTypeDefinitions
                .Where(ctd => tn.ContentTypes.ToList<string>().Contains(ctd.Name))
                .Where(ctd => ctd.DisplayName != null);

            foreach (var ctd in selected)
            {                
                if (tn.AddContentTypeAsParent)
                {
                    builder.Add(new LocalizedString(ctd.DisplayName, ctd.DisplayName), listTypeMenu => { AddContentItems(listTypeMenu, ctd.Name); });
                }
                else
                {
                    AddContentItems(builder, ctd.Name);
                }
            }
        }

        private async void AddContentItems(NavigationBuilder listTypeMenu, string contentTypeName)
        {
            var ListContentItems = await _session.Query<ContentItem, ContentItemIndex>()
                .With<ContentItemIndex>(x => x.Latest)
                .With<ContentItemIndex>(x => x.ContentType == contentTypeName)
                .ListAsync();

            foreach (var ci in ListContentItems)
            {
                var cim = await _contentManager.PopulateAspectAsync<ContentItemMetadata>(ci);

                if ((cim.AdminRouteValues.Any()) && (cim.DisplayText != null))
                {
                    listTypeMenu.Add(new LocalizedString(cim.DisplayText, cim.DisplayText), m => m
                    .Action(cim.AdminRouteValues["Action"] as string, cim.AdminRouteValues["Controller"] as string, cim.AdminRouteValues)
                    .LocalNav());
                }
            }
        }
    }
}
