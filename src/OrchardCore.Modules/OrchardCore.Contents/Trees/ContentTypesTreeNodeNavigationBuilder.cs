using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Services;
using OrchardCore.ContentTree.Trees;
using OrchardCore.Environment.Navigation;
using System.Linq;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata.Models;

namespace OrchardCore.Contents.Trees
{
    public class ContentTypesTreeNodeNavigationBuilder : ITreeNodeNavigationBuilder
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ContentTypesTreeNodeNavigationBuilder(
            IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public string Name => typeof(ContentTypesTreeNode).Name;


        public void BuildNavigation(MenuItem treeNode, NavigationBuilder builder, IEnumerable<ITreeNodeNavigationBuilder> treeNodeBuilders)
        {
            var tn = treeNode as ContentTypesTreeNode;

            if (tn == null)
            {
                return;
            }

            // Add ContentTypes specific children
            var contentTypeDefinitions = _contentDefinitionManager.ListTypeDefinitions().OrderBy(d => d.Name);
            var typesToShow = GetContentTypes(tn);
            foreach (var ctd in typesToShow)
            {
                var rv = new RouteValueDictionary();
                rv.Add("Options.TypeName", ctd.Name);
                builder.Add(new LocalizedString(ctd.DisplayName, ctd.DisplayName), t => t.Action("List", "Admin", "OrchardCore.Contents", rv));
            }


            // Add external children
            foreach (var childTreeNode in tn.Items)
            {   
                var treeBuilder = treeNodeBuilders.Where(x => x.Name == childTreeNode.GetType().Name).FirstOrDefault();
                treeBuilder.BuildNavigation(childTreeNode, builder, treeNodeBuilders);
            }
        }


        private IEnumerable<ContentTypeDefinition> GetContentTypes(ContentTypesTreeNode tn)
        {
            var typesToShow = _contentDefinitionManager.ListTypeDefinitions().
                Where(ctd => ctd.Settings.ToObject<ContentTypeSettings>().Listable);

            if(tn.ShowAll == false)
            {
                typesToShow = typesToShow.Where(ctd => tn.ContentTypes.ToList<string>().Contains(ctd.Name));
            }
            
            return typesToShow.OrderBy( t => t.Name);
        }

        private void AddExternalChildren(MenuItem menuItem , NavigationBuilder builder, IEnumerable<ITreeNodeNavigationBuilder> treeNodeBuilders)
        {

        }
    }

}
