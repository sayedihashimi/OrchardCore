using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.ContentTree.Indexes;
using OrchardCore.Data.Migration;

namespace OrchardCore.ContentTree
{
    public class Migrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable(nameof(ContentTreePresetIndex), table => table
                .Column<string>("Name")
            );

            return 1;
        }

    }
}
