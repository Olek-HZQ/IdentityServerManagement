namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedUserRoleDto : BasePagedDto<UserRoleForPage>
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
    }

    public class UserRoleForPage
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
