using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentTree.Drivers;
using OrchardCore.ContentTree.Indexes;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Services;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Environment.Navigation;
using OrchardCore.Modules;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using YesSql.Indexes;

namespace OrchardCore.ContentTree
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddSingleton<IIndexProvider, ContentTreePresetIndexProvider>();
            services.AddTransient<IDataMigration, Migrations>();

            services.AddScoped<IDisplayDriver<ISite>, ContentTreeSettingsDisplayDriver>();
            
            // dummy link treeNode
            services.AddSingleton<ITreeNodeProviderFactory>(new TreeNodeProviderFactory<DummyLinkTreeNode>());
            services.AddScoped<IDisplayManager<TreeNode>, DisplayManager<TreeNode>>();
            services.AddScoped<IDisplayDriver<TreeNode>, DummyLinkTreeNodeDriver>();

        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
}