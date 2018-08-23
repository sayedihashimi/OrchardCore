using System;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Drivers;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree
{
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            S = localizer;
        }

        public IStringLocalizer S { get; set; }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Add(S["Configuration"], configuration => configuration
                    .Add(S["Settings"], settings => settings
                        .Add(S["Content Tree"], "2" , contentTree => contentTree
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = ContentTreeSettingsDisplayDriver.GroupId })
                            .Permission(Permissions.ManageContentTree)
                            .LocalNav()
                        )))
                .Add(S["Configuration"], content => content
                    .Add(S["Content Tree"], "1.5", layers => layers
                        .Permission(Permissions.ManageContentTree)
                        .Action("List", "Admin", new { area = "OrchardCore.ContentTree" })
                        .LocalNav()
                    ));
        }
    }
}
