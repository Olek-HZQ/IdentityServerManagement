using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiScope;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Core.Entities.IdentityResource;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Services.ApiResource;
using IdentityServer.Admin.Services.ApiScope;
using IdentityServer.Admin.Services.Client;
using IdentityServer.Admin.Services.IdentityResource;
using IdentityServer.Admin.Services.Role;
using IdentityServer.Admin.Services.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Admin.SeedData
{
    public static class SeedDataHelper
    {
        public static async Task InsertIdentitySeedData(this IWebHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();

                    var identityDataConfiguration = scope.ServiceProvider.GetRequiredService<IdentityDataConfiguration>();

                    // insert roles
                    var existsRoles = await roleService.GetAllRolesAsync();
                    if (!existsRoles.Any() && identityDataConfiguration.Roles.Any())
                    {
                        foreach (var role in identityDataConfiguration.Roles)
                        {
                            await roleService.InsertRoleAsync(new Role
                            {
                                Name = role.Name,
                                SystemName = role.SystemName
                            });
                        }
                    }

                    // insert user
                    var existsUsers = await userService.GetAllUsersAsync();
                    if (!existsUsers.Any())
                    {
                        var userId = await userService.InsertUserAsync(new User
                        {
                            SubjectId = Guid.NewGuid().ToString(),
                            Name = identityDataConfiguration.User.Name,
                            Email = identityDataConfiguration.User.Email,
                            Active = true,
                            Deleted = false,
                            CreationTime = DateTime.Now
                        });

                        if (userId > 0)
                        {
                            // insert user role
                            if (identityDataConfiguration.Roles.Any())
                            {
                                var roleSystemNames = identityDataConfiguration.Roles.Select(x => x.SystemName);
                                var insertedRoles = (await roleService.GetAllRolesAsync()).Where(x => roleSystemNames.Contains(x.SystemName)).ToList();
                                if (insertedRoles.Any())
                                {
                                    foreach (var role in insertedRoles)
                                    {
                                        await userService.InsertUserRoleAsync(userId, role.Id);
                                    }
                                }
                            }


                            // insert user claims
                            if (identityDataConfiguration.User.Claims.Any())
                            {
                                foreach (var claim in identityDataConfiguration.User.Claims)
                                {
                                    await userService.InsertUserClaimAsync(new UserClaim
                                    {
                                        UserId = userId,
                                        ClaimType = claim.Type,
                                        ClaimValue = claim.Value
                                    });
                                }
                            }

                            // insert user password
                            await userService.InsertUserPasswordAsync(new UserPassword
                            {
                                UserId = userId,
                                Password = identityDataConfiguration.User.Password
                            });
                        }
                    }
                }
            }
        }

        public static async Task InsertIdentityServerSeedData(this IWebHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var identityResourceService = scope.ServiceProvider.GetRequiredService<IIdentityResourceService>();
                    var apiResourceService = scope.ServiceProvider.GetRequiredService<IApiResourceService>();
                    var apiScopeService = scope.ServiceProvider.GetRequiredService<IApiScopeService>();
                    var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                    var clientSecretService = scope.ServiceProvider.GetRequiredService<IClientSecretService>();

                    var identityServerDataConfiguration = scope.ServiceProvider.GetRequiredService<IdentityServerDataConfiguration>();

                    var existsClients = await clientService.IsExistsAnyClientAsync();

                    if (existsClients == null)
                    {
                        if (identityServerDataConfiguration.IdentityResources.Any())
                        {
                            foreach (var source in identityServerDataConfiguration.IdentityResources)
                            {
                                var identitySource = new IdentityResource
                                {
                                    Name = source.Name,
                                    DisplayName = source.DisplayName,
                                    Description = source.Description,
                                    Required = source.Required,
                                    Emphasize = source.Emphasize
                                };
                                if (source.UserClaims.Any())
                                {
                                    identitySource.UserClaims = source.UserClaims.Select(x => new IdentityResourceClaim
                                    {
                                        Type = x
                                    }).ToList();
                                }

                                await identityResourceService.InsertIdentityResourceAsync(identitySource);
                            }
                        }

                        if (identityServerDataConfiguration.ApiResources.Any())
                        {
                            foreach (var source in identityServerDataConfiguration.ApiResources)
                            {
                                var apiResource = new ApiResource
                                {
                                    Name = source.Name,
                                    DisplayName = source.DisplayName,
                                    Enabled = source.Enabled,
                                    ShowInDiscoveryDocument = source.ShowInDiscoveryDocument
                                };

                                if (source.Scopes.Any())
                                {
                                    apiResource.UserClaims = source.Scopes.Select(x => new ApiResourceClaim
                                    {
                                        Type = x
                                    }).ToList();
                                }

                                await apiResourceService.InsertApiResourceAsync(apiResource);
                            }
                        }

                        if (identityServerDataConfiguration.ApiScopes.Any())
                        {
                            foreach (var item in identityServerDataConfiguration.ApiScopes)
                            {
                                var apiScope = new ApiScope
                                {
                                    Name = item.Name,
                                    DisplayName = item.DisplayName,
                                    Required = item.Required
                                };

                                if (item.UserClaims.Any())
                                {
                                    apiScope.UserClaims = item.UserClaims.Select(x => new ApiScopeClaim
                                    {
                                        Type = x
                                    }).ToList();
                                }

                                await apiScopeService.InsertApiScopeAsync(apiScope);
                            }
                        }

                        if (identityServerDataConfiguration.Clients.Any())
                        {
                            foreach (var item in identityServerDataConfiguration.Clients)
                            {
                                var client = new Client
                                {
                                    ClientId = item.ClientId,
                                    ClientName = item.ClientName,
                                    ClientUri = item.ClientUri,
                                    RequirePkce = item.RequirePkce,
                                    FrontChannelLogoutUri = item.FrontChannelLogoutUri
                                };

                                if (item.AllowedGrantTypes.Any())
                                {
                                    client.AllowedGrantTypes = item.AllowedGrantTypes.Select(x => new ClientGrantType
                                    {
                                        GrantType = x
                                    }).ToList();
                                }

                                if (item.RedirectUris.Any())
                                {
                                    client.RedirectUris = item.RedirectUris.Select(x => new ClientRedirectUri
                                    {
                                        RedirectUri = x
                                    }).ToList();
                                }

                                if (item.PostLogoutRedirectUris.Any())
                                {
                                    client.PostLogoutRedirectUris = item.PostLogoutRedirectUris.Select(x =>
                                        new ClientPostLogoutRedirectUri
                                        {
                                            PostLogoutRedirectUri = x
                                        }).ToList();
                                }

                                if (item.AllowedScopes.Any())
                                {
                                    client.AllowedScopes = item.AllowedScopes.Select(x => new ClientScope
                                    {
                                        Scope = x
                                    }).ToList();
                                }

                                if (item.AllowedCorsOrigins.Any())
                                {
                                    client.AllowedCorsOrigins = item.AllowedCorsOrigins.Select(x => new ClientCorsOrigin
                                    {
                                        Origin = x
                                    }).ToList();
                                }

                                var insertId = await clientService.InsertClientAsync(client);
                                if (insertId > 0 && item.ClientSecrets.Any())
                                {
                                    var secrets = item.ClientSecrets.Select(x => new ClientSecret
                                    {
                                        ClientId = insertId,
                                        Value = x.Value.Sha256()
                                    }).ToList();
                                    foreach (var secret in secrets)
                                    {
                                        await clientSecretService.InsertClientSecretAsync(secret);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
