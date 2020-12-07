using System;

namespace IdentityServer.Admin.Core.Dtos.Client
{
    public class PagedClientSecretDto : BasePagedDto<ClientSecretDetailForPage>
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }
    }

    public class ClientSecretDetailForPage
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Type { get; set; } = "SharedSecret";

        public string Value { get; set; }

        public DateTime? Expiration { get; set; }

        public DateTime Created { get; set; }
    }
}
