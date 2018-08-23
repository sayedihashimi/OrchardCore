using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;

namespace OrchardCore.ContentTree.Services
{
    public interface ITreeNodeProviderFactory
    {
        string Name { get; }
        TreeNode Create();
    }

    public class TreeNodeProviderFactory<TTreeNode> : ITreeNodeProviderFactory where TTreeNode : TreeNode, new()
    {
        private static readonly string TypeName = typeof(TTreeNode).Name;

        public string Name => TypeName;

        public TreeNode Create()
        {
            return new TTreeNode();
        }
    }
}
