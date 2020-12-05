using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityServer.Admin.Models
{
    public class UserRoleModel
    {
        public UserRoleModel()
        {
            AvailableRoles = new List<SelectListItem>();
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        public IList<SelectListItem> AvailableRoles { get; set; }
    }
}
