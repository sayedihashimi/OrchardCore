using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using OrchardCore.Admin;
using OrchardCore.ContentTree.Models;
using OrchardCore.ContentTree.Services;
using OrchardCore.ContentTree.ViewModels;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Settings;
using YesSql;

namespace OrchardCore.ContentTree.Controllers
{
    [Admin]
    public class ContentTreeNodeController : Controller, IUpdateModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDisplayManager<TreeNode> _displayManager;
        private readonly IEnumerable<ITreeNodeProviderFactory> _factories;
        private readonly ISession _session;
        private readonly ISiteService _siteService;
        private readonly INotifier _notifier;

        public ContentTreeNodeController(
            IAuthorizationService authorizationService,
            IDisplayManager<TreeNode> displayManager,
            IEnumerable<ITreeNodeProviderFactory> factories,
            ISession session,
            ISiteService siteService,
            IShapeFactory shapeFactory,
            IStringLocalizer<ContentTreeNodeController> stringLocalizer,
            IHtmlLocalizer<ContentTreeNodeController> htmlLocalizer,
            INotifier notifier)
        {
            _displayManager = displayManager;
            _factories = factories;
            _authorizationService = authorizationService;
            _session = session;
            _siteService = siteService;
            New = shapeFactory;
            _notifier = notifier;
            T = stringLocalizer;
            H = htmlLocalizer;
        }

        public dynamic New { get; set; }
        public IStringLocalizer T { get; set; }
        public IHtmlLocalizer H { get; set; }

        public async Task<IActionResult> Create(int id, string type)
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

            var treeNode = _factories.FirstOrDefault(x => x.Name == type)?.Create();

            if (treeNode == null)
            {
                return NotFound();
            }

            treeNode.Id = Guid.NewGuid().ToString("n");

            var model = new EditContentTreePresetTreeNodeViewModel
            {
                ContentTreePresetId = id,
                TreeNode = treeNode,
                TreeNodeId = treeNode.Id,
                TreeNodeType = type,
                Editor = await _displayManager.BuildEditorAsync(treeNode, updater: this, isNew: true)
            };

            model.Editor.DeploymentStep = treeNode;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EditContentTreePresetTreeNodeViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(model.ContentTreePresetId);

            if (contentTreePreset == null)
            {
                return NotFound();
            }

            var treeNode = _factories.FirstOrDefault(x => x.Name == model.TreeNodeType)?.Create();

            if (treeNode == null)
            {
                return NotFound();
            }

            dynamic editor = await _displayManager.UpdateEditorAsync(treeNode, updater: this, isNew: true);
            editor.TreeNode = treeNode;

            if (ModelState.IsValid)
            {
                treeNode.Id = model.TreeNodeId;
                treeNode.Name = model.TreeNodeType;
                contentTreePreset.TreeNodes.Add(treeNode);
                _session.Save(contentTreePreset);

                _notifier.Success(H["TreeNode added successfully"]);
                return RedirectToAction("Display", "Admin", new { id = model.ContentTreePresetId });
            }

            model.Editor = editor;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Edit(int id, string treeNodeId)
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

            var treeNode = contentTreePreset.TreeNodes.FirstOrDefault(x => String.Equals(x.Id, treeNodeId, StringComparison.OrdinalIgnoreCase));

            if (treeNode == null)
            {
                return NotFound();
            }

            var model = new EditContentTreePresetTreeNodeViewModel
            {
                ContentTreePresetId = id,
                TreeNode = treeNode,
                TreeNodeId = treeNode.Id,
                TreeNodeType = treeNode.GetType().Name,
                Editor = await _displayManager.BuildEditorAsync(treeNode, updater: this, isNew: false)
            };

            model.Editor.TreeNode = treeNode;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditContentTreePresetTreeNodeViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageContentTree))
            {
                return Unauthorized();
            }

            var contentTreePreset = await _session.GetAsync<ContentTreePreset>(model.ContentTreePresetId);

            if (contentTreePreset == null)
            {
                return NotFound();
            }

            var treeNode = contentTreePreset.TreeNodes.FirstOrDefault(x => String.Equals(x.Id, model.TreeNodeId, StringComparison.OrdinalIgnoreCase));

            if (treeNode == null)
            {
                return NotFound();
            }

            var editor = await _displayManager.UpdateEditorAsync(treeNode, updater: this, isNew: false);

            if (ModelState.IsValid)
            {
                _session.Save(contentTreePreset);

                _notifier.Success(H["Tree node updated successfully"]);
                return RedirectToAction("Display", "Admin", new { id = model.ContentTreePresetId});
            }

            _notifier.Error(H["The tree node has validation errors"]);
            model.Editor = editor;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string treeNodeId)
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

            var treeNode = contentTreePreset.TreeNodes.FirstOrDefault(x => String.Equals(x.Id, treeNodeId, StringComparison.OrdinalIgnoreCase));

            if (treeNode == null)
            {
                return NotFound();
            }

            contentTreePreset.TreeNodes.Remove(treeNode);
            _session.Save(contentTreePreset);

            _notifier.Success(H["Tree node deleted successfully"]);

            return RedirectToAction("Display", "Admin", new { id });
        }
    }
}

