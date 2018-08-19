using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;

namespace OrchardCore.ContentTree.ViewModels
{
    public class ContentTreePresetIndexViewModel
    {
        public IList<ContentTreePresetEntry> ContentTreePresets { get; set; }
        public ContentTreePresetIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }
    }


    public class ContentTreePresetEntry
    {
        public ContentTreePreset ContentTreePreset { get; set; }
        public bool IsChecked { get; set; }
    }

}
