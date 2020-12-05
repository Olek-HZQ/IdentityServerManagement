namespace IdentityServer.Admin.Models
{
    public class DeleteUserRoleModel
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }
    }
}
