namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedClientClaimDto : BasePagedDto<ClientClaimForPage>
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }
    }
}
