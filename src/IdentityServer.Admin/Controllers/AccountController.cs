using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Admin.Controllers
{
    [Authorize]
    public class AccountController :Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new List<string> { "Cookies", "oidc" });
        }
    }
}
