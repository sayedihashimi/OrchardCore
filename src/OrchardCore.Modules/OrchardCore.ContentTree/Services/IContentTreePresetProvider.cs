using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentTree.Models;

namespace OrchardCore.ContentTree.Services
{
    public interface IContentTreePresetProvider
    {
        Task<ContentTreePreset> GetPreset(int id);
        Task<ContentTreePreset> GetDefaultPreset();
    }
}
