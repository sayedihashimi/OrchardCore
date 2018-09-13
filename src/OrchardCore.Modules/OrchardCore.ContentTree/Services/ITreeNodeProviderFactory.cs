using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Services
{
    public interface ITreeNodeProviderFactory
    {
        string Name { get; }
        MenuItem Create();
    }

    public class TreeNodeProviderFactory<TTreeNode> : ITreeNodeProviderFactory where TTreeNode : MenuItem, new()
    {
        private static readonly string TypeName = typeof(TTreeNode).Name;

        public string Name => TypeName;

        public MenuItem Create()
        {
            return new TTreeNode();
        }
    }
}
