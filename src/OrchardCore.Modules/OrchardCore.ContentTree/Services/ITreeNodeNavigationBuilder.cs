using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Services
{
    public interface ITreeNodeNavigationBuilder
    {
        // This Name will be used to determine if the treeNode passed has to be handled.
        // The builder will handle  only the treeNodes whose typeName equals this name.
        string Name { get; }

        void BuildNavigation(MenuItem treeNode, NavigationBuilder builder, IEnumerable<ITreeNodeNavigationBuilder> treeNodeBuilders);
        
    }
}
