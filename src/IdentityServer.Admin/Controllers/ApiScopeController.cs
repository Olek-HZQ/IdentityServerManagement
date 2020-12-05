using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Controllers
{
    public class ApiScopeController : BaseController
    {
        private readonly IApiScopeService _apiScopeService;

        public ApiScopeController(IApiScopeService apiScopeService)
        {
            _apiScopeService = apiScopeService;
        }

        public async Task<IActionResult> Index(string search, int? page)
        {
            var pagedApiScopes = await _apiScopeService.GetPagedAsync(search, page ?? 1);

            ViewBag.PagedApiScopes = pagedApiScopes;

            ViewBag.Search = search;

            return View(new ApiScopeModel());
        }

        public IActionResult Create()
        {
            return View(new ApiScopeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApiScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            model.UserClaims = model.UserClaimsItems.Deserialize<List<string>>()?.Select(x => new ApiScopeClaimModel()
            {
                Type = x
            }).ToList();

            await _apiScopeService.InsertApiScopeAsync(CommonMappers.Mapper.Map<ApiScope>(model));

            SuccessNotification("Api 作用域添加成功", "成功");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var apiScope = await _apiScopeService.GetApiScopeByIdAsync(id);

            if (apiScope == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var apiScopeModel = CommonMappers.Mapper.Map<ApiScopeModel>(apiScope);

            return View(apiScopeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApiScopeModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var apiScope = await _apiScopeService.GetApiScopeByIdAsync(model.Id);

            if (apiScope == null)
            {
                return RedirectToAction(nameof(Index));
            }

            model.UserClaims = model.UserClaimsItems.Deserialize<List<string>>()?.Select(x => new ApiScopeClaimModel()
            {
                Type = x
            }).ToList();

            apiScope = CommonMappers.Mapper.Map<ApiScope>(model);

            var updatedResult = await _apiScopeService.UpdateApiScopeAsync(apiScope);

            if (updatedResult)
            {
                SuccessNotification($"Api 作用域编辑成功", "成功");

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var apiScope = await _apiScopeService.GetApiScopeByIdAsync(id);

            if (apiScope == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = CommonMappers.Mapper.Map<ApiScopeModel>(apiScope);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApiScopeModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _apiScopeService.DeleteApiScopeAsync(CommonMappers.Mapper.Map<ApiScope>(model));

            if (result)
            {
                SuccessNotification($"Api 作用域删除成功", "成功");
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
