using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models;
using IdentityServer.Admin.Services.ApiResource;
using Newtonsoft.Json;

namespace IdentityServer.Admin.Controllers
{
    public class ApiResourceController : BaseController
    {
        private readonly IApiResourceService _apiResourceService;

        public ApiResourceController(IApiResourceService apiResourceService)
        {
            _apiResourceService = apiResourceService;
        }

        public async Task<IActionResult> Index(string search, int? page)
        {
            var pagedApiSources = await _apiResourceService.GetPagedAsync(search, page ?? 1);

            ViewBag.Search = search;

            return View(pagedApiSources);
        }

        public IActionResult Create()
        {
            var model = new ApiResourceModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApiResourceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserClaims = !string.IsNullOrEmpty(model.UserClaimsItems)
                ? JsonConvert.DeserializeObject<List<string>>(model.UserClaimsItems).Select(x =>
                    new ApiResourceClaimModel
                    {
                        Type = x
                    }).ToList()
                : new List<ApiResourceClaimModel>();

            await _apiResourceService.InsertApiResourceAsync(CommonMappers.Mapper.Map<ApiResource>(model));
            SuccessNotification($"Api 资源【{model.Name}】添加成功", "成功");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var apiResource = await _apiResourceService.GetApiResourceByIdAsync(id);

            if (apiResource == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(CommonMappers.Mapper.Map<ApiResourceModel>(apiResource));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApiResourceModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var apiResource = await _apiResourceService.GetApiResourceByIdAsync(model.Id);

            if (apiResource == null)
            {
                return RedirectToAction(nameof(Index));
            }

            model.UserClaims = model.UserClaimsItems.Deserialize<List<string>>()?.Select(x => new ApiResourceClaimModel
            {
                Type = x
            }).ToList();

            apiResource = CommonMappers.Mapper.Map<ApiResource>(model);

            var updatedResult = await _apiResourceService.UpdateApiResourceAsync(apiResource);

            if (updatedResult)
            {
                SuccessNotification("Api 资源编辑成功", "成功");
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var apiResource = await _apiResourceService.GetApiResourceByIdAsync(id);

            if (apiResource == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = CommonMappers.Mapper.Map<ApiResourceModel>(apiResource);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApiResourceModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _apiResourceService.DeleteApiResourceAsync(CommonMappers.Mapper.Map<ApiResource>(model));

            if (result)
            {
                SuccessNotification("Api 资源删除成功", "成功");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
