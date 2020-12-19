using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Services.ApiResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Controllers
{
    public class ApiResourceScopeController : BaseController
    {
        private readonly IApiResourceScopeService _apiResourceScopeService;
        private readonly ILocalizationService _localizationService;

        public ApiResourceScopeController(IApiResourceScopeService apiResourceScopeService,ILocalizationService localizationService)
        {
            _apiResourceScopeService = apiResourceScopeService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Index(int apiResourceId, int? page)
        {
            var pagedApiResourceScope = await _apiResourceScopeService.GetPagedAsync(apiResourceId, page ?? 1);

            ViewBag.PagedApiResourceScope = pagedApiResourceScope;

            return View(new ApiResourceScopeModel
            {
                ApiResourceId = pagedApiResourceScope.ApiResourceId,
                ApiResourceName = pagedApiResourceScope.ApiResourceName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApiResourceScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { apiResourceId = model.ApiResourceId });
            }

            var insertedResult = await _apiResourceScopeService.InsertApiResourceScopeAsync(CommonMappers.Mapper.Map<ApiResourceScope>(model));

            if (insertedResult > 0)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("ApiResourceScope.Added"));
            }

            return RedirectToAction(nameof(Index), new { apiResourceId = model.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int apiResourceId)
        {
            var apiResourceScope = await _apiResourceScopeService.GetApiResourceScopeByIdAsync(id);

            if (apiResourceScope == null)
            {
                return RedirectToAction(nameof(Index), new { apiResourceId });
            }

            var model = CommonMappers.Mapper.Map<ApiResourceScopeModel>(apiResourceScope);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApiResourceScopeModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _apiResourceScopeService.DeleteApiResourceScopeAsync(CommonMappers.Mapper.Map<ApiResourceScope>(model));

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("ApiResourceScope.Deleted"));
                return RedirectToAction(nameof(Index), new { apiResourceId = model.ApiResourceId });
            }

            return View(model);
        }
    }
}
