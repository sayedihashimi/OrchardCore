using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentTree.Models;

namespace OrchardCore.ContentTree.ViewModels
{
    public class EditTreeNodeViewModel
    {
        public int ContentTreePresetId { get; set; }
        public string TreeNodeId { get; set; }
        public string TreeNodeType { get; set; }
        public dynamic Editor { get; set; }

        [BindNever]
        public TreeNode TreeNode { get; set; }

    }
}
