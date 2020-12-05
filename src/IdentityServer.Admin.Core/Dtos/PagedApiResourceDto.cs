namespace IdentityServer.Admin.Core.Dtos
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
