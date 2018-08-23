using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.ViewModels;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Entities.DisplayManagement;
using OrchardCore.Settings;
using YesSql;
using System.Linq;
using OrchardCore.ContentTree.Indexes;

namespace OrchardCore.ContentTree.Drivers
{
    public class ContentTreeSettingsDisplayDriver : SectionDisplayDriver<ISite, ContentTreeSettings>
    {
        public const string GroupId = "contenttree";
        private readonly ISession _session;        
        
        public ContentTreeSettingsDisplayDriver(ISession session)
        {
            _session = session;
        }        

        public override IDisplayResult Edit(ContentTreeSettings section, BuildEditorContext context)
        {
            var contentTreePresets = _session
                .Query<ContentTreePreset, ContentTreePresetIndex>()
                .ListAsync().Result
                .ToDictionary(x => x.Id, x => x.Name);            

            
            return Initialize<ContentTreeSettingsViewModel>("ContentTreeSettings_Edit", model =>
            {
                model.ContentTreePreset = section.ContentTreePresetId;                
                model.ContentTreePresets = contentTreePresets;
            }).Location("Content:2").OnGroup("contenttree");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTreeSettings section, BuildEditorContext context)
        {
            if (context.GroupId == "contenttree")
            {
                var model = new ContentTreeSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                section.ContentTreePresetId = model.ContentTreePreset;                
            }

            return await EditAsync(section, context);
        }
    }
}
