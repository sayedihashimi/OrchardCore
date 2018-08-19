using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Models;
using YesSql.Indexes;

namespace OrchardCore.ContentTree.Indexes
{
    public class ContentTreePresetIndex : MapIndex
    {
        public string Name { get; set; }
    }

    public class ContentTreePresetIndexProvider: IndexProvider<ContentTreePreset>
    {
        public override void Describe(DescribeContext<ContentTreePreset> context)
        {
            context.For<ContentTreePresetIndex>()
                .Map(contentTreePreset =>
                {
                    return new ContentTreePresetIndex
                    {
                        Name = contentTreePreset.Name
                    };
                });
        }

    }
}
