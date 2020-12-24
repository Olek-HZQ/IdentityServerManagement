using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.Users
{
    [Table(TableNameConstant.UserRoleMap)]
    public class UserRoleMapping
    {
        public int UserId { get; set; }

        [Computed]
        public virtual User User { get; set; }

        public int RoleId { get; set; }

        [Computed]
        public virtual Role Role { get; set; }
    }
}
