﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Services.ApiResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Controllers
{
    public class ApiResourcePropertyController : BaseController
    {
        private readonly IApiResourcePropertyService _apiResourcePropertyService;
        private readonly ILocalizationService _localizationService;

        public ApiResourcePropertyController(IApiResourcePropertyService apiResourcePropertyService, ILocalizationService localizationService)
        {
            _apiResourcePropertyService = apiResourcePropertyService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Index(int apiResourceId, int? page)
        {
            var pagedApiResourceProperty = await _apiResourcePropertyService.GetPagedAsync(apiResourceId, page ?? 1);

            ViewBag.PagedApiResourceProperty = pagedApiResourceProperty;

            return View(new ApiResourcePropertyModel
            {
                ApiResourceId = pagedApiResourceProperty.ApiResourceId,
                ApiResourceName = pagedApiResourceProperty.ApiResourceName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApiResourcePropertyModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { apiResourceId = model.ApiResourceId });
            }

            var insertedResult = await _apiResourcePropertyService.InsertApiResourceProperty(CommonMappers.Mapper.Map<ApiResourceProperty>(model));

            if (insertedResult > 0)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("ApiResourceProperty.Added"));
            }

            return RedirectToAction(nameof(Index), new { apiResourceId = model.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int apiResourceId)
        {
            var apiResourceProperty = await _apiResourcePropertyService.GetApiResourcePropertyById(id);

            if (apiResourceProperty == null)
            {
                return RedirectToAction(nameof(Index), new { apiResourceId });
            }

            var model = CommonMappers.Mapper.Map<ApiResourcePropertyModel>(apiResourceProperty);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApiResourcePropertyModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _apiResourcePropertyService.DeleteApiResourceProperty(CommonMappers.Mapper.Map<ApiResourceProperty>(model));

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("ApiResourceProperty.Deleted"));
                return RedirectToAction(nameof(Index), new { apiResourceId = model.ApiResourceId });
            }

            return View(model);
        }
    }
}
