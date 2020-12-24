using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.Users
{
    [Table(TableNameConstant.Role)]
    public class Role
    {
        public Role()
        {
            UserRoleMaps = new List<UserRoleMapping>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string SystemName { get; set; }

        [Computed]
        public List<UserRoleMapping> UserRoleMaps { get; set; }
    }
}
