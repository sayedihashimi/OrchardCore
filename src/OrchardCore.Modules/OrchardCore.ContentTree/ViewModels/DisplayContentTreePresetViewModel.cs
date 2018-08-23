using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;

namespace OrchardCore.ContentTree.ViewModels
{
    public class DisplayContentTreePresetViewModel
    {   
            public ContentTreePreset ContentTreePreset { get; set; }
            public IEnumerable<dynamic> Items { get; set; }
            public IDictionary<string, dynamic> Thumbnails { get; set; }     
    }
}
