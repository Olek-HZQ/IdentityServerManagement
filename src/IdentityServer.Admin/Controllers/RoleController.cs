using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.Role;
using IdentityServer.Admin.Services.Localization;
using IdentityServer.Admin.Services.Role;

namespace IdentityServer.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly ILocalizationService _localizationService;

        public RoleController(IRoleService roleService,ILocalizationService localizationService)
        {
            _roleService = roleService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Index(string search, int? page)
        {
            ViewBag.Search = search;
            var pagedRoles = await _roleService.GetPagedAsync(search, page ?? 1);

            ViewBag.PagedRoles = pagedRoles;

            return View(new RoleModel());
        }

        public IActionResult Create()
        {
            return View(new RoleModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await _roleService.InsertRoleAsync(CommonMappers.Mapper.Map<Role>(model));

            SuccessNotification(await _localizationService.GetResourceAsync("Roles.Added"));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var userModel = CommonMappers.Mapper.Map<RoleModel>(role);

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleService.GetRoleByIdAsync(model.Id);

            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            role = CommonMappers.Mapper.Map<Role>(model);

            var updatedResult = await _roleService.UpdateRoleAsync(role);

            if (updatedResult)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("Roles.Updated"));

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = CommonMappers.Mapper.Map<RoleModel>(role);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RoleModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleService.DeleteRoleAsync(CommonMappers.Mapper.Map<Role>(model));

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("Roles.Deleted"));
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
