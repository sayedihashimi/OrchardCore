using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Services;
using OrchardCore.Environment.Navigation;
using System.Linq;

namespace OrchardCore.ContentTree
{
    public class AdminMenu : INavigationProvider
    {
        private readonly ContentTreeNavigationProviderCoordinator _contentTreeNavigationProvider;

        public AdminMenu(ContentTreeNavigationProviderCoordinator contentTreeNavigationProvider,
            IStringLocalizer<AdminMenu> localizer)
        {
            _contentTreeNavigationProvider = contentTreeNavigationProvider;
            S = localizer;
        }

        public IStringLocalizer S { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Configuration and settings menus for the ContentTree module
            builder.Add(S["Configuration"], content => content
                    .Add(S["Content Tree"], "1.5", layers => layers
                        .Permission(Permissions.ManageContentTree)
                        .Action("List", "Admin", new { area = "OrchardCore.ContentTree" })
                        .LocalNav()
                    ));

            // This is the entry point for the contentTree. 
            // Dynamically generated menus that will appear under the root "Content" admin menu.
            _contentTreeNavigationProvider.BuildNavigation("contenttree", builder);
        }
    }
}
