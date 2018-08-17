using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Environment.Navigation;
using System;
using System.Linq;
using YesSql;

namespace OrchardCore.Lists
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public AdminMenu(
            IStringLocalizer<AdminMenu> localizer,
            IContentDefinitionManager contentDefinitionManager,
            IContentManager contentManager,
            ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _session = session;
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // retrieve all content types that have a list part.
            //todo: do we need a permission for this?
            var contentTypesWithList = _contentDefinitionManager.ListTypeDefinitions()
                .Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Creatable)
                .Where(ctd => ctd.Parts.Any(p => p.PartDefinition.Name.Equals("ListPart", StringComparison.OrdinalIgnoreCase)))
                .OrderBy(ctd => ctd.DisplayName);

            if (contentTypesWithList.Any())
            {
                builder.Add(T["Content"], contentMenu =>
                {
                    contentMenu.Add(T["Lists"], listsMenu => {

                        foreach (var ctd in contentTypesWithList)
                        {
                            listsMenu.Add(new LocalizedString(ctd.DisplayName, ctd.DisplayName), async listTypeMenu =>
                            {
                                var ListContentItems = await _session.Query<ContentItem, ContentItemIndex>()
                                    .With<ContentItemIndex>(x => x.Latest)
                                    .With<ContentItemIndex>(x => x.ContentType == ctd.Name)
                                    .ListAsync();
                                
                                foreach (var ci in ListContentItems)
                                {
                                    var cim = await _contentManager.PopulateAspectAsync<ContentItemMetadata>(ci);                                    
                                    if (cim.AdminRouteValues.Any())
                                    {
                                        listTypeMenu.Add(new LocalizedString(cim.DisplayText, cim.DisplayText), m => m
                                        .Action(cim.AdminRouteValues["Action"] as string, cim.AdminRouteValues["Controller"] as string, cim.AdminRouteValues)
                                        .LocalNav());
                                    }
                                }
                            });
                        };
                    });
                });
            };
        }        
    }
}