using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Helpers;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.User;
using IdentityServer.Admin.Services.Role;
using IdentityServer.Admin.Services.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServer.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        #region User

        public async Task<IActionResult> Index(string search, int? page)
        {
            ViewBag.Search = search;
            var pagedUsers = await _userService.GetPagedAsync(search, page ?? 1);

            ViewBag.PagedUsers = pagedUsers;

            return View(new UserModel());
        }

        public IActionResult Create()
        {
            return View(new UserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.InsertUserAsync(CommonMappers.Mapper.Map<User>(model));

            SuccessNotification("用户添加成功", "成功");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var userModel = CommonMappers.Mapper.Map<UserModel>(user);

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetUserByIdAsync(model.Id);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            user = CommonMappers.Mapper.Map<User>(model);

            var updatedResult = await _userService.UpdateUserAsync(user);

            if (updatedResult)
            {
                SuccessNotification("用户编辑成功", "成功");

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = CommonMappers.Mapper.Map<UserModel>(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(UserModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _userService.DeleteUserAsync(CommonMappers.Mapper.Map<User>(model));

            if (result)
            {
                SuccessNotification("用户删除成功", "成功");
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion

        #region User Role

        [HttpGet]
        public async Task<IActionResult> UserRole(int id, int? page)
        {
            var pagedUserRoles = await _userService.GetPagedUserRoleAsync(id, page ?? 1);

            var model = new UserRoleModel
            {
                UserId = pagedUserRoles.UserId,
                UserName = pagedUserRoles.UserName,
                AvailableRoles = (await _roleService.GetAllRolesAsync()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            ViewBag.PagedUserRoles = pagedUserRoles;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserRole(UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isExistsUserRole = await _userService.IsUserRoleExistsAsync(model.UserId, model.RoleId);
            if (isExistsUserRole)
            {
                CreateNotification(NotificationHelper.AlertType.Info, "该角色已经添加", "成功");
                return RedirectToAction(nameof(UserRole), new { id = model.UserId });
            }

            var insertedResult = await _userService.InsertUserRoleAsync(model.UserId, model.RoleId);

            if (insertedResult > 0)
            {
                SuccessNotification("用户角色添加成功", "成功");
                return RedirectToAction(nameof(UserRole), new { id = model.UserId });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserRole(int userId, int roleId)
        {
            var userRole = await _userService.GetUserRoleByAsync(userId, roleId);

            if (userRole == null)
            {
                return RedirectToAction(nameof(UserRole), new { id = userId });
            }

            return View(CommonMappers.Mapper.Map<DeleteUserRoleModel>(userRole));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserRole(DeleteUserRoleModel model)
        {
            var userRole = await _userService.GetUserRoleByAsync(model.UserId, model.RoleId);

            if (userRole == null)
            {
                return RedirectToAction(nameof(Edit), new { id = model.UserId });
            }

            var deletedResult = await _userService.DeleteUserRoleByAsync(model.UserId, model.RoleId);

            if (deletedResult)
            {
                SuccessNotification("用户角色删除成功", "成功");
                return RedirectToAction(nameof(UserRole), new { id = model.UserId });
            }

            return View(model);
        }

        #endregion

        #region User Claims

        [HttpGet]
        public async Task<IActionResult> UserClaim(int id, int? page)
        {
            var pagedUserClaims = await _userService.GetPagedUserClaimAsync(id, page ?? 1);

            ViewBag.PagedUserClaims = pagedUserClaims;

            var model = new UserClaimModel
            {
                UserId = pagedUserClaims.UserId,
                UserName = pagedUserClaims.UserName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserClaim(UserClaimModel model)
        {
            if (!ModelState.IsValid)
            {
                var pagedUserClaims = await _userService.GetPagedUserClaimAsync(model.UserId, 1);

                ViewBag.PagedUserClaims = pagedUserClaims;
                return View(model);
            }

            var isExistsUserClaim = await _userService.IsUserClaimExistsAsync(model.UserId, model.ClaimType, model.ClaimValue);
            if (isExistsUserClaim)
            {
                CreateNotification(NotificationHelper.AlertType.Info, "该声明已经添加", "成功");
                return RedirectToAction(nameof(UserClaim), new { id = model.UserId });
            }

            await _userService.InsertUserClaimAsync(CommonMappers.Mapper.Map<UserClaim>(model));

            SuccessNotification("用户声明添加成功", "成功");
            return RedirectToAction(nameof(UserClaim), new { id = model.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserClaim(int id, int userId)
        {
            var userClaim = await _userService.GetUserClaimByIdAsync(id);

            if (userClaim == null)
            {
                return RedirectToAction(nameof(UserClaim), new { id = userId });
            }

            return View(new UserClaimModel
            {
                Id = userClaim.Id,
                UserId = userClaim.UserId,
                ClaimType = userClaim.ClaimType,
                ClaimValue = userClaim.ClaimValue
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserClaim(UserClaimModel model)
        {
            var userClaim = await _userService.GetUserClaimByIdAsync(model.Id);

            if (userClaim == null)
            {
                return RedirectToAction(nameof(UserClaim), new { id = model.UserId });
            }

            if (userClaim.UserId != model.UserId)
            {
                return RedirectToAction(nameof(UserClaim), new { id = model.UserId });
            }

            var deletedResult = await _userService.DeleteUserClaimByIdAsync(model.Id);

            if (deletedResult)
            {
                SuccessNotification("用户声明删除成功", "成功");
                return RedirectToAction(nameof(UserClaim), new { id = model.UserId });
            }

            return View(model);
        }

        #endregion

        #region Change Password

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var userPassword = await _userService.GetPasswordByUserIdAsync(user.Id);

            var model = new UserPasswordModel
            {
                UserId = user.Id,
                Name = user.Name
            };

            if (userPassword != null)
            {
                model.Id = userPassword.Id;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserPasswordModel model)
        {
            var currentUserPassword = await _userService.GetUserPasswordAsync(model.Id);

            var userPassword = CommonMappers.Mapper.Map<UserPassword>(model);

            if (currentUserPassword == null)
            {
                var insertedResult = await _userService.InsertUserPasswordAsync(userPassword);

                if (insertedResult > 0)
                    SuccessNotification("用户密码修改成功", "成功");

                return RedirectToAction(nameof(Index));
            }

            if (currentUserPassword.UserId != model.UserId)
            {
                return RedirectToAction(nameof(Index));
            }

            var updatedResult = await _userService.UpdateUserPasswordAsync(userPassword);
            if (updatedResult > 0)
            {
                SuccessNotification("用户密码修改成功", "成功");

                return RedirectToAction(nameof(Edit), new { id = model.UserId });
            }

            return View(model);
        }

        #endregion
    }
}
