using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;
using YesSql;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace OrchardCore.ContentTree.Services
{
    // This class retrieves all instances of "ITreeNodeNavigationBuilder"
    // Those are classes that add new "menuItems" to a "NavigationBuilder" using custom logic specific to the module that register them.
    // This class handles their inclusion on the admin menu.
    // This class is itself one more INavigationProvider so it can be called from this module's AdminMenu.cs
    public class ContentTreeNavigationProviderCoordinator : INavigationProvider
    {
        //private readonly ISession _session;
        private readonly IContentTreePresetProvider _contentTreePresetProvider;
        private readonly IEnumerable<ITreeNodeNavigationBuilder> _treeNodeNavigationBuilders;


        public ContentTreeNavigationProviderCoordinator(
            IContentTreePresetProvider contentTreePresetProvider,
            IEnumerable<ITreeNodeNavigationBuilder> treeNodeNavigationBuilders,
            ILogger<ContentTreeNavigationProviderCoordinator> logger)
        {
            _contentTreePresetProvider = contentTreePresetProvider;
            _treeNodeNavigationBuilders = treeNodeNavigationBuilders;
        }

        public ILogger Logger { get; set; }


        // We only add them if the caller knows what it's doing (by using the magic string "contenttree").
        // todo: use a public constant for the string
        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "contenttree", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var menuItems = _contentTreePresetProvider.GetDefaultPreset().Result?.MenuItems.ToArray();

            if (menuItems == null)
            {
                return;
            }

            foreach (MenuItem menuItem in menuItems)
            {
                var treeNodeCustomLogicBuilder = _treeNodeNavigationBuilders.Where(x => x.Name == menuItem.GetType().Name).FirstOrDefault();
                if (treeNodeCustomLogicBuilder != null)
                {
                    treeNodeCustomLogicBuilder.BuildNavigation(menuItem, builder, _treeNodeNavigationBuilders);
                }
                else
                {
                    Logger.LogError("No Builder registered for treeNode of type '{TreeNodeName}'", menuItem.GetType().Name);
                }
            }
        }

    }
}
