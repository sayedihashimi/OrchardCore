using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Models
{
    //todo: move to a abstraction project?
    public class ContentTreePreset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MenuItem> MenuItems { get; } = new List<MenuItem>();
    }
}
