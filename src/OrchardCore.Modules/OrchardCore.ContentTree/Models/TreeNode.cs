using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Models
{
    public abstract class TreeNode 
    {
        public string Id { get; set; }
        public string Name { get; set; }

        //public abstract void BuildNavigation(string name, NavigationBuilder builder);
    }
}
