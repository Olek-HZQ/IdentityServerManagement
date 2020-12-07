using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Dapper.Repositories.ApiResource;
using IdentityServer.Admin.Dapper.Repositories.ApiScope;
using IdentityServer.Admin.Dapper.Repositories.IdentityResource;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Admin.Services.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;

        public ResourceStore(IIdentityResourceRepository identityResourceRepository, IApiResourceRepository apiResourceRepository, IApiScopeRepository apiScopeRepository)
        {
            _identityResourceRepository = identityResourceRepository;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = await _identityResourceRepository.GetIdentityResourcesByScopeNameAsync(scopeNames.ToArray());

            var result = identityResources.Select(x => new IdentityServer4.Models.IdentityResource
            {
                Enabled = x.Enabled,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Required = x.Required,
                Emphasize = x.Emphasize,
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                UserClaims = x.UserClaims.Select(y => y.Type).ToList(),
                Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
            }).ToArray();

            return result;
        }

        public async Task<IEnumerable<IdentityServer4.Models.ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var apiScopes = await _apiScopeRepository.GetApiScopesByNameAsync(scopeNames.ToArray());

            var result = apiScopes.Select(x => new IdentityServer4.Models.ApiScope
            {
                Enabled = x.Enabled,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Required = x.Required,
                Emphasize = x.Emphasize,
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                UserClaims = x.UserClaims.Select(c => c.Type).ToList(),
                Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
            }).ToArray();

            return result;
        }

        private static ICollection<string> Convert(string sourceMember)
        {
            var list = new HashSet<string>();
            if (!string.IsNullOrWhiteSpace(sourceMember))
            {
                sourceMember = sourceMember.Trim();
                foreach (var item in sourceMember.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var apiResources = await _apiResourceRepository.GetApiResourcesByScopeNameAsync(scopeNames.ToArray());

            var result = apiResources.Select(x => new IdentityServer4.Models.ApiResource
            {
                Enabled = x.Enabled,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                AllowedAccessTokenSigningAlgorithms = Convert(x.AllowedAccessTokenSigningAlgorithms),
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                ApiSecrets = x.Secrets.Select(s => new Secret
                {
                    Description = s.Description,
                    Value = s.Value,
                    Expiration = s.Expiration,
                    Type = s.Type
                }).ToArray(),
                Scopes = x.Scopes.Select(s => s.Scope).ToArray(),
                UserClaims = x.UserClaims.Select(c => c.Type).ToArray(),
                Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
            });

            return result;
        }

        public async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var apiResources = await _apiResourceRepository.GetApiResourcesByNameAsync(apiResourceNames.ToArray());

            var result = apiResources.Select(x => new IdentityServer4.Models.ApiResource
            {
                Enabled = x.Enabled,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                AllowedAccessTokenSigningAlgorithms = Convert(x.AllowedAccessTokenSigningAlgorithms),
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                ApiSecrets = x.Secrets.Select(s => new Secret
                {
                    Description = s.Description,
                    Value = s.Value,
                    Expiration = s.Expiration,
                    Type = s.Type
                }).ToArray(),
                Scopes = x.Scopes.Select(s => s.Scope).ToArray(),
                UserClaims = x.UserClaims.Select(c => c.Type).ToArray(),
                Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
            });

            return result;
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var identity = await _identityResourceRepository.GetAllIdentityResourceList();
            IEnumerable<IdentityServer4.Models.IdentityResource> identityResources = new List<IdentityServer4.Models.IdentityResource>();
            if (identity != null && identity.Any())
            {
                identityResources = identity.Select(x => new IdentityServer4.Models.IdentityResource
                {
                    Enabled = x.Enabled,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    Required = x.Required,
                    Emphasize = x.Emphasize,
                    ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                    UserClaims = x.UserClaims.Select(y => y.Type).ToList(),
                    Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
                }).ToArray();
            }

            var apis = await _apiResourceRepository.GetApiResourcesByNameAsync(null);
            IEnumerable<IdentityServer4.Models.ApiResource> apiResources = new List<IdentityServer4.Models.ApiResource>();
            if (apis != null && apis.Any())
            {
                apiResources = apis.Select(x => new IdentityServer4.Models.ApiResource
                {
                    Enabled = x.Enabled,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    AllowedAccessTokenSigningAlgorithms = Convert(x.AllowedAccessTokenSigningAlgorithms),
                    ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                    ApiSecrets = x.Secrets.Select(s => new Secret
                    {
                        Description = s.Description,
                        Value = s.Value,
                        Expiration = s.Expiration,
                        Type = s.Type
                    }).ToArray(),
                    Scopes = x.Scopes.Select(s => s.Scope).ToArray(),
                    UserClaims = x.UserClaims.Select(c => c.Type).ToArray(),
                    Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
                });
            }

            var scopes = await _apiScopeRepository.GetAllApiScopeListAsync();
            IEnumerable<IdentityServer4.Models.ApiScope> apiScopes = new List<IdentityServer4.Models.ApiScope>();
            if (scopes != null && scopes.Any())
            {
                apiScopes = scopes.Select(x => new IdentityServer4.Models.ApiScope
                {
                    Enabled = x.Enabled,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    Required = x.Required,
                    Emphasize = x.Emphasize,
                    ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                    UserClaims = x.UserClaims.Select(c => c.Type).ToList(),
                    Properties = x.Properties.ToDictionary(k => k.Key, v => v.Value)
                }).ToArray();
            }

            var result = new Resources(identityResources, apiResources, apiScopes);

            return result;
        }
    }
}
