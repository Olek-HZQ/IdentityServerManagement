namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedApiResourceScopeDto : BasePagedDto<ApiResourceScopeForPage>
    {
        public int ApiResourceId { get; set; }

        public string ApiResourceName { get; set; }
    }

    public class ApiResourceScopeForPage
    {
        public int Id { get; set; }

        public string Scope { get; set; }
    }
}
