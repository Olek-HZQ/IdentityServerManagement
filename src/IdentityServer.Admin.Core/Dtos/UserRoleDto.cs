namespace IdentityServer.Admin.Core.Dtos
{
    public class UserRoleDto
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }
    }
}
