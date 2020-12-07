using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Helpers;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models;
using IdentityServer.Admin.Services.PersistedGrant;

namespace IdentityServer.Admin.Controllers
{
    public class PersistedGrantController : BaseController
    {
        private readonly IPersistedGrantService _persistedGrantService;

        public PersistedGrantController(IPersistedGrantService persistedGrantService)
        {
            _persistedGrantService = persistedGrantService;
        }

        public async Task<IActionResult> Index(string search, int? page)
        {
            ViewBag.Search = search;
            var persistedGrants = await _persistedGrantService.GetPagedAsync(search, page ?? 1);

            return View(persistedGrants);
        }

        public async Task<IActionResult> Detail(string id, int? page)
        {
            var persistedGrants = await _persistedGrantService.GetPagedByUserAsync(id, page ?? 1);

            ViewBag.PersistedGrants = persistedGrants;

            return View(new PersistedGrantModel
            {
                SubjectId = id
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var persistedGrant = await _persistedGrantService.GetByKeyAsync(UrlHelper.QueryStringUnSafeHash(id));

            if (persistedGrant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(CommonMappers.Mapper.Map<PersistedGrantModel>(persistedGrant));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(PersistedGrantModel model)
        {
            var result = await _persistedGrantService.DeleteByKeyAsync(UrlHelper.QueryStringUnSafeHash(model.Key));

            if (result)
            {
                SuccessNotification("删除成功", "成功");

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll(PersistedGrantModel model)
        {
            await _persistedGrantService.DeleteAllBySubjectIdAsync(model.SubjectId);

            SuccessNotification("删除成功", "成功");

            return RedirectToAction(nameof(Index));
        }
    }
}
