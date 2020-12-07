namespace IdentityServer.Admin.Core.Dtos.ApiResource
{
    public class PagedApiResourceDto : BasePagedDto<ApiResourceForPage>
    {
    }

    public class ApiResourceForPage
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
