namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedIdentityResourceDto : BasePagedDto<IdentityResourceForPage>
    {
    }

    public class IdentityResourceForPage
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
