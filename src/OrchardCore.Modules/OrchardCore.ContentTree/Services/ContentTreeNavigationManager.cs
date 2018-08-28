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
    public class ContentTreeNavigationManager : INavigationProvider
    {
        //private readonly ISession _session;
        private readonly IContentTreePresetProvider _contentTreePresetProvider;
        private readonly IEnumerable<ITreeNodeNavigationBuilder> _navigationBuilders;

        public ContentTreeNavigationManager(
            IContentTreePresetProvider contentTreePresetProvider,
            IEnumerable<ITreeNodeNavigationBuilder> navigationBuilders,
            IStringLocalizer<ContentTreeNavigationManager> localizer,
            ILogger<ContentTreeNavigationManager> logger)
        {
            _contentTreePresetProvider = contentTreePresetProvider;
            _navigationBuilders = navigationBuilders;
            S = localizer;
        }

        public IStringLocalizer S { get; set; }
        public ILogger Logger { get; set; }


        // We only handle here menus whose name is "contenttree".
        public void BuildNavigation(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "contenttree", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var treeNodes = _contentTreePresetProvider.GetDefaultPreset().Result?.TreeNodes.ToArray();

            if (treeNodes == null)
            {
                return;
            }

            foreach (TreeNode treeNode in treeNodes)
            {
                var nb = _navigationBuilders.Where(x => x.Name == treeNode.Name).FirstOrDefault();
                if (nb != null)
                {
                    nb.BuildNavigation(treeNode, builder);
                }
                else
                {
                    Logger.LogError("No Builder registered for treeNode of type '{TreeNodeName}'", treeNode.Name);                         
                }
            }

            
        }
    }
}
