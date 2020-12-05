using Dapper.Contrib.Extensions;

namespace IdentityServer.Admin.Core.Entities
{
    [Table("UserRoleMap")]
    public class UserRoleMap
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
