using System;
using System.Collections.Generic;
using System.Text;

namespace OrchardCore.ContentTree.ViewModels
{
    public class ContentTreeSettingsViewModel
    {
        public int ContentTreePreset { get; set; }        
        public Dictionary<int, string> ContentTreePresets { get; set; }        
    }
}
