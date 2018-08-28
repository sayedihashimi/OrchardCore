using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;

namespace OrchardCore.Menu.Trees
{
    public class MenuTreeNodeViewModel
    {
        // selected menu contentItems
        public string[] Selected { get; set; }

        // available menu contentItems as a dictionary of id/displayname
        [BindNever]
        public Dictionary<string, string> AvailableMenuContentItems { get; set; }
    }
}
