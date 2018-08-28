using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentTree.Indexes;
using OrchardCore.ContentTree.Models;
using OrchardCore.Entities;
using OrchardCore.Settings;
using YesSql;

namespace OrchardCore.ContentTree.Services
{
    public class ContentTreePresetProvider : IContentTreePresetProvider
    {
        private readonly ISession _session;
        private readonly ISiteService _siteService;

        public ContentTreePresetProvider(ISession session,
            ISiteService siteService)
        {
            _session = session;
            _siteService = siteService;
        }

        public async Task<ContentTreePreset> GetDefaultPreset()
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();

            var presetId = siteSettings.As<ContentTreeSettings>().ContentTreePresetId;

           return await GetPreset(presetId);
            
        }

        public async Task<ContentTreePreset> GetPreset(int id)
        {
            return await _session.GetAsync<ContentTreePreset>(id);
            
        }
    }
}
