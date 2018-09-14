using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;

using OrchardCore.ContentManagement.Records;
using YesSql;

using Microsoft.Extensions.Logging;

using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using OrchardCore.Settings;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Navigation;
using OrchardCore.ContentTree.ViewModels;
using Microsoft.AspNetCore.Routing;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Indexes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.ContentTree.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OrchardCore.Environment.Navigation;

namespace OrchardCore.ContentTree.Controllers
{
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ISession _session;
        private readonly IDisplayManager<MenuItem> _displayManager;
        private readonly IEnumerable<ITreeNodeProviderFactory> _factories;
        private readonly ISiteService _siteService;
        private readonly INotifier _notifier;

        public AdminController(
            IAuthorizationService authorizationService,
            ISession session,
            IDisplayManager<MenuItem> displayManager,
            ISiteService siteService,
            IEnumerable<ITreeNodeProviderFactory> factories,
            IShapeFactory shapeFactory,
            INotifier notifier,
            IStringLocalizer<AdminController> stringLocalizer,
            IHtmlLocalizer<AdminController> htmlLocalizer,
            ILogger<AdminController> logger)
        {
            _authorizationService = authorizationService;
            _session = session;
            _siteService = siteService;
            _displayManager = displayManager;
            _factories = factories;
            New = shapeFactory;
            _notifier = notifier;

            T = stringLocalizer;
            H = htmlLocalizer;
            Logger = logger;
        }

        public IStringLocalizer T { get; set; }
        public IHtmlLocalizer H { get; set; }
        public ILogger Logger { get; set; }
        public dynamic New { get; set; }

        public async Task<IActionResult> List(ContentTreePresetIndexOptions options, PagerParameters pagerParameters)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);

            // default options
            if (options == null)
            {
                options = new ContentTreePresetIndexOptions();
            }

            var contentTreePresets = _session.Query<ContentTreePreset, ContentTreePresetIndex>();

            if (!string.IsNullOrWhiteSpace(options.Search))
            {
                contentTreePresets = contentTreePresets.Where(dp => dp.Name.Contains(options.Search));
            }

            var count = await contentTreePresets.CountAsync();

            var startIndex = pager.GetStartIndex();
            var pageSize = pager.PageSize;
            IEnumerable<ContentTreePreset> results = new List<ContentTreePreset>();

            //todo: handle the case where there is a deserialization exception on some of the presets.
            // load at least the ones without error. Provide a way to delete the ones on error.
            try
            {
                results = await contentTreePresets
                .Skip(startIndex)
                .Take(pageSize)
                .ListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error when retrieving the list of content tree presets");
                _notifier.Error(H["Error when retrieving the list of content tree presets"]);
            }


            // Maintain previous route data when generating page links
            var routeData = new RouteData();
            routeData.Values.Add("Options.Search", options.Search);

            var pagerShape = (await New.Pager(pager)).TotalItemCount(count).RouteData(routeData);

            var model = new ContentTreePresetIndexViewModel
            {
                ContentTreePresets = results.Select(x => new ContentTreePresetEntry { ContentTreePreset = x }).ToList(),
                Options = options,
                Pager = pagerShape
            };

            return View(model);
        }

        public async Task<IActionResult> Display(int id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(id);

            if (contentTreePreset == null)
            {
                return NotFound();
            }

            var thumbnails = new Dictionary<string, dynamic>();
            foreach (var factory in _factories)
            {
                var treeNode = factory.Create();
                dynamic thumbnail = await _displayManager.BuildDisplayAsync(treeNode, this, "TreeThumbnail");
                thumbnail.TreeNode = treeNode;
                thumbnails.Add(factory.Name, thumbnail);
            }

            var model = new DisplayContentTreePresetViewModel
            {
                ContentTreePreset = contentTreePreset,
                Thumbnails = thumbnails,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MoveTreeNode(int contentTreePresetId, string nodeToMoveId,
            string destinationNodeId, int position)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(contentTreePresetId);

            if ((contentTreePreset == null) || (contentTreePreset.MenuItems == null))
            {
                return NotFound();
            }


            var nodeToMove = contentTreePreset.GetMenuItemById(nodeToMoveId);
            if (nodeToMove == null)
            {
                return NotFound();
            }

            var destinationNode = contentTreePreset.GetMenuItemById(destinationNodeId); // don't check for null. When null the item will be moved to the root.

            if (contentTreePreset.RemoveMenuItem(nodeToMove) == false)
            {   
                
            }            

            if(contentTreePreset.InsertMenuItemAt(nodeToMove, destinationNode, position) == false)
            {
                return StatusCode(500);
            }
            
            _session.Save(contentTreePreset);
            
            return RedirectToAction(nameof(Display), new { id = contentTreePresetId });
        }


        public async Task<IActionResult> Create()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var model = new CreateContentTreeViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContentTreeViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var contentTreePreset = new ContentTreePreset { Name = model.Name, Enabled = model.Enabled };

                _session.Save(contentTreePreset);
                return RedirectToAction(nameof(List));
            }


            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(id);

            if (contentTreePreset == null)
            {
                return NotFound();
            }

            var model = new EditContentTreeViewModel
            {
                Name = contentTreePreset.Name,
                Enabled = contentTreePreset.Enabled            
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditContentTreeViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(model.Id);

            if (contentTreePreset == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError(nameof(EditContentTreeViewModel.Name), T["The name is mandatory."]);
                }
            }

            if (ModelState.IsValid)
            {
                contentTreePreset.Name = model.Name;
                contentTreePreset.Enabled = model.Enabled;

                _session.Save(contentTreePreset);

                _notifier.Success(H["Content Tree preset updated successfully"]);

                return RedirectToAction(nameof(List));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(id);

            if (contentTreePreset == null)
            {
                return NotFound();
            }

            _session.Delete(contentTreePreset);

            _notifier.Success(H["Content tree preset deleted successfully"]);

            return RedirectToAction(nameof(List));
        }
    }
}
