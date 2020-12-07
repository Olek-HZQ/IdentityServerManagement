namespace IdentityServer.Admin.Core.Dtos.User
{
    public class PagedUserClaimDto : BasePagedDto<UserClaimForPage>
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
    }

    public class UserClaimForPage
    {
        public int Id { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
