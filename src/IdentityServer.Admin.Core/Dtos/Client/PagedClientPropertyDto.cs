namespace IdentityServer.Admin.Core.Dtos.Client
{
    public class PagedClientPropertyDto : BasePagedDto<ClientPropertyForPage>
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }
    }
}
