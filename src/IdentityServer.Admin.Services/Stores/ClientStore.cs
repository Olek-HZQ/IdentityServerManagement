using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Dapper.Repositories.Client;
using IdentityServer4.Models;
using ClientClaim = IdentityServer4.Models.ClientClaim;
using Secret = IdentityServer4.Models.Secret;
using TokenExpiration = IdentityServer4.Models.TokenExpiration;
using TokenUsage = IdentityServer4.Models.TokenUsage;

namespace IdentityServer.Admin.Services.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IClientRepository _clientRepository;

        public ClientStore(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            var localClient = await _clientRepository.GetClientByClientId(clientId);

            if (localClient != null)
            {
                var client = new IdentityServer4.Models.Client
                {
                    Enabled = localClient.Enabled,
                    ClientId = localClient.ClientId,
                    ProtocolType = localClient.ProtocolType,
                    RequireClientSecret = localClient.RequireClientSecret,
                    ClientName = localClient.ClientName,
                    Description = localClient.Description,
                    ClientUri = localClient.ClientUri,
                    LogoUri = localClient.LogoUri,
                    RequireConsent = localClient.RequireConsent,
                    AllowRememberConsent = localClient.AllowRememberConsent,
                    AlwaysIncludeUserClaimsInIdToken = localClient.AlwaysIncludeUserClaimsInIdToken,
                    RequirePkce = localClient.RequirePkce,
                    AllowPlainTextPkce = localClient.AllowPlainTextPkce,
                    AllowAccessTokensViaBrowser = localClient.AllowAccessTokensViaBrowser,
                    FrontChannelLogoutUri = localClient.FrontChannelLogoutUri,
                    FrontChannelLogoutSessionRequired = localClient.FrontChannelLogoutSessionRequired,
                    BackChannelLogoutUri = localClient.BackChannelLogoutUri,
                    BackChannelLogoutSessionRequired = localClient.BackChannelLogoutSessionRequired,
                    AllowOfflineAccess = localClient.AllowOfflineAccess,
                    IdentityTokenLifetime = localClient.IdentityTokenLifetime,
                    AccessTokenLifetime = localClient.AccessTokenLifetime,
                    AuthorizationCodeLifetime = localClient.AuthorizationCodeLifetime,
                    ConsentLifetime = localClient.ConsentLifetime,
                    AbsoluteRefreshTokenLifetime = localClient.AbsoluteRefreshTokenLifetime,
                    SlidingRefreshTokenLifetime = localClient.SlidingRefreshTokenLifetime,
                    RefreshTokenUsage = (TokenUsage)localClient.RefreshTokenUsage,
                    UpdateAccessTokenClaimsOnRefresh = localClient.UpdateAccessTokenClaimsOnRefresh,
                    RefreshTokenExpiration = (TokenExpiration)localClient.RefreshTokenExpiration,
                    AccessTokenType = (AccessTokenType)localClient.AccessTokenType,
                    EnableLocalLogin = localClient.EnableLocalLogin,
                    IncludeJwtId = localClient.IncludeJwtId,
                    AlwaysSendClientClaims = localClient.AlwaysSendClientClaims,
                    ClientClaimsPrefix = localClient.ClientClaimsPrefix,
                    PairWiseSubjectSalt = localClient.PairWiseSubjectSalt,
                    UserSsoLifetime = localClient.UserSsoLifetime,
                    UserCodeType = localClient.UserCodeType,
                    DeviceCodeLifetime = localClient.DeviceCodeLifetime,
                    AllowedCorsOrigins = localClient.AllowedCorsOrigins.Select(x => x.Origin).ToList(),
                    AllowedGrantTypes = localClient.AllowedGrantTypes.Select(x => x.GrantType).ToList(),
                    AllowedScopes = localClient.AllowedScopes.Select(x => x.Scope).ToList(),
                    Claims = localClient.Claims.Any()
                        ? localClient.Claims.Select(x => new ClientClaim { Type = x.Type, Value = x.Value, }).ToList()
                        : new List<ClientClaim>(),
                    ClientSecrets = localClient.ClientSecrets.Any()
                        ? localClient.ClientSecrets.Select(x => new Secret
                        {
                            Type = x.Type,
                            Value = x.Value,
                            Expiration = x.Expiration,
                            Description = x.Description
                        }).ToList()
                        : new List<Secret>(),
                    IdentityProviderRestrictions = localClient.IdentityProviderRestrictions.Select(x => x.Provider).ToList(),
                    PostLogoutRedirectUris = localClient.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri).ToList(),
                    Properties = localClient.Properties.ToDictionary(x => x.Key, y => y.Value),
                    RedirectUris = localClient.RedirectUris.Select(x => x.RedirectUri).ToList()
                };

                return client;
            }

            return default;
        }
    }
}
