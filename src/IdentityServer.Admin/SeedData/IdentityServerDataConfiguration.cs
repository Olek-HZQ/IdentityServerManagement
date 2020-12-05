using System.Collections.Generic;

namespace IdentityServer.Admin.SeedData
{
    public class IdentityServerDataConfiguration
    {
        public IdentityServerDataConfiguration()
        {
            IdentityResources = new List<IdentityResourceSeedData>();
            ApiResources = new List<ApiResourceSeedData>();
            ApiScopes = new List<ApiScopeSeedData>();
            Clients = new List<ClientSeedData>();
        }

        public List<IdentityResourceSeedData> IdentityResources { get; set; }

        public List<ApiResourceSeedData> ApiResources { get; set; }

        public List<ApiScopeSeedData> ApiScopes { get; set; }

        public List<ClientSeedData> Clients { get; set; }
    }

    public class IdentityResourceSeedData
    {
        public IdentityResourceSeedData()
        {
            UserClaims = new List<string>();
        }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<string> UserClaims { get; set; }
    }

    public class ApiResourceSeedData
    {
        public ApiResourceSeedData()
        {
            Scopes = new List<string>();
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> Scopes { get; set; }
    }

    public class ApiScopeSeedData
    {
        public ApiScopeSeedData()
        {
            UserClaims = new List<string>();
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Required { get; set; }

        public List<string> UserClaims { get; set; }
    }

    public class ClientSeedData
    {
        public ClientSeedData()
        {
            AllowedGrantTypes = new List<string>();
            ClientSecrets = new List<ClientSecretSeedData>();
            RedirectUris = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            AllowedCorsOrigins = new List<string>();
            AllowedScopes = new List<string>();
        }

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientUri { get; set; }

        public List<string> AllowedGrantTypes { get; set; }

        public bool RequirePkce { get; set; }

        public List<ClientSecretSeedData> ClientSecrets { get; set; }

        public List<string> RedirectUris { get; set; }

        public string FrontChannelLogoutUri { get; set; }

        public List<string> PostLogoutRedirectUris { get; set; }

        public List<string> AllowedCorsOrigins { get; set; }

        public List<string> AllowedScopes { get; set; }
    }

    public class ClientSecretSeedData
    {
        public string Value { get; set; }
    }
}
