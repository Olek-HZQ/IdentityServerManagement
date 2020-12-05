namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedClientPropertyDto : BasePagedDto<ClientPropertyForPage>
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }
    }
}
