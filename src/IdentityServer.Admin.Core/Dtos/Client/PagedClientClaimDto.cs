namespace IdentityServer.Admin.Core.Dtos.Client
{
    public class PagedClientClaimDto : BasePagedDto<ClientClaimForPage>
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }
    }
}
