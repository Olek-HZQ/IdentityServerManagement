namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedApiScopeDto : BasePagedDto<ApiScopeForPage>
    {
    }

    public class ApiScopeForPage
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
