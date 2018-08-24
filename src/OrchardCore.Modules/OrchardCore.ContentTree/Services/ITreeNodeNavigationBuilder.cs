using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Services
{
    public interface ITreeNodeNavigationBuilder
    {
        string Name { get; }
        void BuildNavigation(TreeNode treeNode, NavigationBuilder builder);
    }
}
