using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using Serilog;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.SqlServer
{
    public class ClientRepository : MssqlRepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(DbConnectionConfiguration dbMapConfig) : base((dbMapConfig))
        {
        }

        private async Task CombineClient(Client client, IDbSession session)
        {
            var scopeQuery = new Query(AttributeExtension.GetTableAttributeName<ClientScope>())
                        .Select("Scope").Where("ClientId", "=", client.Id);
            var scopeSqlResult = GetSqlResult(scopeQuery);

            var postLogoutRedirectUriQuery = new Query(AttributeExtension.GetTableAttributeName<ClientPostLogoutRedirectUri>())
                    .Select("PostLogoutRedirectUri").Where("ClientId", "=", client.Id);
            var postLogoutRedirectUriSqlResult = GetSqlResult(postLogoutRedirectUriQuery);

            var clientIdPRestrictionQuery = new Query(AttributeExtension.GetTableAttributeName<ClientIdPRestriction>())
                .Select("Provider").Where("ClientId", "=", client.Id);
            var clientIdRestrictionResult = GetSqlResult(clientIdPRestrictionQuery);

            var redirectUrisQuery = new Query(AttributeExtension.GetTableAttributeName<ClientRedirectUri>())
                .Select("RedirectUri").Where("ClientId", "=", client.Id);
            var redirectUrisSqlResult = GetSqlResult(redirectUrisQuery);

            var corsOriginsQuery = new Query(AttributeExtension.GetTableAttributeName<ClientCorsOrigin>())
                .Select("Origin").Where("ClientId", "=", client.Id);
            var corsOriginsSqlResult = GetSqlResult(corsOriginsQuery);

            var grantTypesQuery = new Query(AttributeExtension.GetTableAttributeName<ClientGrantType>())
                .Select("GrantType").Where("ClientId", "=", client.Id);
            var grantTypesSqlResult = GetSqlResult(grantTypesQuery);

            var claimsQuery = new Query(AttributeExtension.GetTableAttributeName<ClientClaim>())
                .Select("Id", "Type", "Value").Where("ClientId", "=", client.Id);
            var claimsSqlResult = GetSqlResult(claimsQuery);

            var secretsQuery = new Query(AttributeExtension.GetTableAttributeName<ClientSecret>())
                .Select("Id", "Type", "Value").Where("ClientId", "=", client.Id);
            var secretsSqlResult = GetSqlResult(secretsQuery);

            var propertiesQuery = new Query(AttributeExtension.GetTableAttributeName<ClientProperty>())
                .Select("Id", "Key", "Value").Where("ClientId", "=", client.Id);
            var propertiesSqlResult = GetSqlResult(propertiesQuery);

            var multiQuery = await session.Connection.QueryMultipleAsync(
                $"{scopeSqlResult.Sql};{postLogoutRedirectUriSqlResult.Sql};{clientIdRestrictionResult.Sql};{redirectUrisSqlResult.Sql};{corsOriginsSqlResult.Sql};{grantTypesSqlResult.Sql};{claimsSqlResult.Sql};{secretsSqlResult.Sql};{propertiesSqlResult.Sql}",
                scopeSqlResult.NamedBindings);

            client.AllowedScopes = (await multiQuery.ReadAsync<ClientScope>()).ToList();
            client.PostLogoutRedirectUris = (await multiQuery.ReadAsync<ClientPostLogoutRedirectUri>()).ToList();
            client.IdentityProviderRestrictions = (await multiQuery.ReadAsync<ClientIdPRestriction>()).ToList();
            client.RedirectUris = (await multiQuery.ReadAsync<ClientRedirectUri>()).ToList();
            client.AllowedCorsOrigins = (await multiQuery.ReadAsync<ClientCorsOrigin>()).ToList();
            client.AllowedGrantTypes = (await multiQuery.ReadAsync<ClientGrantType>()).ToList();
            client.Claims = multiQuery.Read<ClientClaim>().ToList();
            client.ClientSecrets = multiQuery.Read<ClientSecret>().ToList();
            client.Properties = multiQuery.Read<ClientProperty>().ToList();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            IDbSession session = DbSession;

            #region TODO: No working
            /*
            string[] columns = {
                "c.AbsoluteRefreshTokenLifetime", "c.AccessTokenLifetime", "c.ConsentLifetime",
                "c.AllowAccessTokensViaBrowser", "c.AllowOfflineAccess", "c.AllowPlainTextPkce",
                "c.AllowRememberConsent", "AlwaysIncludeUserClaimsInIdToken",
                "c.AlwaysSendClientClaims", "c.AuthorizationCodeLifetime",
                "c.FrontChannelLogoutUri", "c.BackChannelLogoutSessionRequired",
                "scope.Scope", "cpl.PostLogoutRedirectUri", "cru.RedirectUri",
                "cco.Origin", "cgt.GrantType","cc.Type","cc.Value","cs.Description","cp.ClientId"
            };

            Query query = new Query("Clients as c").Select(columns)
                .LeftJoin("ClientScopes as scope", "c.Id", "scope.ClientId")
                .LeftJoin("ClientPostLogoutRedirectUris as cpl", "c.Id", "cpl.ClientId")
                .LeftJoin("ClientRedirectUris as cru", "cpl.ClientId", "cru.ClientId")
                .LeftJoin("ClientCorsOrigins as cco", "cru.ClientId", "cco.ClientId")
                .LeftJoin("ClientGrantTypes as cgt", "cco.ClientId", "cgt.ClientId")
                .LeftJoin("ClientClaims as cc", "cgt.ClientId", "cc.ClientId")
                .LeftJoin("ClientSecrets as cs", "cc.ClientId", "cs.ClientId")
                .LeftJoin("ClientProperties as cp", "cs.ClientId", "cp.ClientId")
                .Where("c.Id", "=", id);

            SqlResult sqlResult = GetSqlResult(query);

            var clientDictionary = new Dictionary<int, Client>();

            var result = session.Connection.Query(sqlResult.Sql, new[]
            {
                    typeof(Client), typeof(List<ClientScope>), typeof(List<ClientPostLogoutRedirectUri>),
                    typeof(List<ClientRedirectUri>), typeof(List<ClientCorsOrigin>), typeof(List<ClientGrantType>),
                    typeof(List<ClientClaim>), typeof(List<ClientSecret>), typeof(List<ClientProperty>)
                }, obj =>
                {
                    Client client = obj[0] as Client;

                    if (client != null)
                    {
                        if (!clientDictionary.TryGetValue(client.Id, out Client clientEntity))
                        {
                            clientDictionary.Add(client.Id, clientEntity = client);
                        }

                        if (obj[1] is ClientScope clientScope)
                        {
                            if (clientEntity.AllowedScopes.All(x => x.Id != clientScope.Id))
                            {
                                clientEntity.AllowedScopes.Add(clientScope);
                            }
                        }

                        if (obj[2] is ClientPostLogoutRedirectUri clientPostLogoutRedirectUri)
                        {
                            if (clientEntity.PostLogoutRedirectUris.All(x => x.Id != clientPostLogoutRedirectUri.Id))
                            {
                                clientEntity.PostLogoutRedirectUris.Add(clientPostLogoutRedirectUri);
                            }
                        }

                        if (obj[3] is ClientRedirectUri clientRedirectUri)
                        {
                            if (clientEntity.RedirectUris.All(x => x.Id != clientRedirectUri.Id))
                            {
                                clientEntity.RedirectUris.Add(clientRedirectUri);
                            }
                        }

                        if (obj[4] is ClientCorsOrigin clientCorsOrigin)
                        {
                            if (clientEntity.AllowedCorsOrigins.All(x => x.Id != clientCorsOrigin.Id))
                            {
                                clientEntity.AllowedCorsOrigins.Add(clientCorsOrigin);
                            }
                        }

                        if (obj[5] is ClientGrantType clientGrantType)
                        {
                            if (clientEntity.AllowedGrantTypes.All(x => x.Id != clientGrantType.Id))
                            {
                                clientEntity.AllowedGrantTypes.Add(clientGrantType);
                            }
                        }

                        if (obj[6] is ClientClaim clientClaim)
                        {
                            if (clientEntity.Claims.All(x => x.Id != clientClaim.Id))
                            {
                                clientEntity.Claims.Add(clientClaim);
                            }
                        }

                        if (obj[7] is ClientSecret clientSecret)
                        {
                            if (clientEntity.ClientSecrets.All(x => x.Id != clientSecret.Id))
                            {
                                clientEntity.ClientSecrets.Add(clientSecret);
                            }
                        }

                        if (obj[8] is ClientProperty clientProperty)
                        {
                            if (clientEntity.Properties.All(x => x.Id != clientProperty.Id))
                            {
                                clientEntity.Properties.Add(clientProperty);
                            }
                        }
                    }

                    return client;
                }, sqlResult.NamedBindings, splitOn: "Scope,PostLogoutRedirectUri,RedirectUri,Origin,GrantType,Type,Description,ClientId").FirstOrDefault();
            */

            #endregion

            try
            {
                var clientQuery = new Query(TableName).Select("*").Where("Id", "=", id);
                var clientSqlResult = GetSqlResult(clientQuery);

                var client = await GetFirstOrDefaultAsync(clientSqlResult.Sql, clientSqlResult.NamedBindings);

                if (client != null)
                {
                    await CombineClient(client, session);
                }

                return client;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<Client> IsExistsAnyClientAsync()
        {
            var query = new Query(TableName).Select("*").Take(1);
            var sqlResult = GetSqlResult(query, false);

            return await base.GetFirstOrDefaultAsync(sqlResult.Sql, sqlResult.NamedBindings);
        }

        public async Task<Client> GetClientByClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return null;
            }

            IDbSession session = DbSession;

            try
            {
                var clientQuery = new Query(TableName).Select("*").Where("ClientId", "=", clientId);
                var clientSqlResult = GetSqlResult(clientQuery);

                var client = await GetFirstOrDefaultAsync(clientSqlResult.Sql, clientSqlResult.NamedBindings);

                if (client != null)
                {
                    await CombineClient(client, session);
                }

                return client;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<List<string>> GetClientCorsOriginList(string origin)
        {
            IDbSession session = DbSession;

            var query = new Query($"{TableName} as t").Join($"{AttributeExtension.GetTableAttributeName<ClientCorsOrigin>()} as t1", "t.Id", "t1.ClientId")
                .Select("t1.Origin").WhereContains("t1.Origin", origin);

            var sqlResult = GetSqlResult(query);

            var origins = (await session.Connection.QueryAsync<string>(sqlResult.Sql, sqlResult.NamedBindings)).ToList();

            session.Dispose();

            return origins;
        }

        public List<Client> GetAllClientsAsync()
        {
            Query query = new Query(TableName).Select("*");
            SqlResult sqlResult = GetSqlResult(query);

            return GetListAsync(sqlResult.Sql).Result.ToList();
        }

        public PagedClientDto GetPagedClients(string search, int page, int pageSize)
        {
            Query totalCountQuery = new Query(TableName);
            Query resultQuery = new Query(TableName).Select("Id", "ClientId", "ClientName");

            if (!string.IsNullOrEmpty(search))
            {
                totalCountQuery = totalCountQuery.WhereContains("ClientId", search).OrWhereContains("ClientName", search);
                resultQuery = resultQuery.WhereContains("ClientId", search).OrWhereContains("ClientName", search);
            }

            totalCountQuery = totalCountQuery.AsCount();
            SqlResult totalCountResult = GetSqlResult(totalCountQuery);

            resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Take(pageSize);
            SqlResult clientResult = GetSqlResult(resultQuery);

            IDbSession session = DbSession;

            try
            {
                int totalCount = session.Connection.QueryFirst<int>(totalCountResult.Sql, totalCountResult.NamedBindings);

                var result = session.Connection.Query<ClientPagedListForPage>(clientResult.Sql, clientResult.NamedBindings).ToList();

                return new PagedClientDto
                {
                    DataPagedList = result,
                    TotalCount = totalCount,
                    PageSize = pageSize
                };
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<int> InsertAsync(Client entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;
            session.BeginTrans();
            IDbTransaction transaction = session.Transaction;
            try
            {
                var updatedClientResult = await session.Connection.InsertAsync(entity, transaction);

                if (updatedClientResult > 0)
                {
                    if (entity.AllowedGrantTypes.Any())
                    {
                        string grantTypeTableName = AttributeExtension.GetTableAttributeName<ClientGrantType>();
                        var grantTypeColumns = new[]
                        {
                            "GrantType", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, grantTypeTableName, grantTypeColumns,
                            entity.AllowedGrantTypes.Select(x => x.GrantType));
                    }

                    if (entity.AllowedScopes.Any())
                    {
                        string scopeTableName = AttributeExtension.GetTableAttributeName<ClientScope>();
                        var scopeColumns = new[]
                        {
                            "Scope", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, scopeTableName, scopeColumns,
                            entity.AllowedScopes.Select(x => x.Scope));
                    }

                    if (entity.RedirectUris.Any())
                    {
                        string redirectUriTableName = AttributeExtension.GetTableAttributeName<ClientRedirectUri>();
                        var redirectUriColumns = new[]
                        {
                            "RedirectUri", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, redirectUriTableName, redirectUriColumns,
                            entity.RedirectUris.Select(x => x.RedirectUri));
                    }

                    if (entity.PostLogoutRedirectUris.Any())
                    {
                        string postLogoutRedirectUriTableName = AttributeExtension.GetTableAttributeName<ClientPostLogoutRedirectUri>();
                        var postLogoutRedirectUriColumns = new[]
                        {
                            "PostLogoutRedirectUri", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, postLogoutRedirectUriTableName,
                            postLogoutRedirectUriColumns,
                            entity.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri));
                    }

                    if (entity.IdentityProviderRestrictions.Any())
                    {
                        string identityProviderRestrictionsTableName = AttributeExtension.GetTableAttributeName<ClientIdPRestriction>();
                        var identityProviderRestrictionsColumns = new[]
                        {
                            "Provider", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, identityProviderRestrictionsTableName,
                            identityProviderRestrictionsColumns,
                            entity.IdentityProviderRestrictions.Select(x => x.Provider));
                    }

                    if (entity.AllowedCorsOrigins.Any())
                    {
                        string allowedCorsOriginsTableName = AttributeExtension.GetTableAttributeName<ClientCorsOrigin>();
                        var allowedCorsOriginsColumns = new[]
                        {
                            "Origin", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, allowedCorsOriginsTableName,
                            allowedCorsOriginsColumns, entity.AllowedCorsOrigins.Select(x => x.Origin));
                    }
                }

                transaction.Commit();
                return updatedClientResult;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                Log.Error($"ClientRepository >> InsertAsync Error: {ex.Message}");

                return 0;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<bool> UpdateAsync(Client entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;
            session.BeginTrans();
            IDbTransaction transaction = session.Transaction;
            try
            {
                var updatedClientResult = await session.Connection.UpdateAsync(entity, transaction);

                if (updatedClientResult)
                {
                    if (entity.AllowedGrantTypes.Any())
                    {
                        string grantTypeTableName = AttributeExtension.GetTableAttributeName<ClientGrantType>();
                        var grantTypeColumns = new[]
                        {
                            "GrantType", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, grantTypeTableName, grantTypeColumns,
                            entity.AllowedGrantTypes.Select(x => x.GrantType));
                    }

                    if (entity.AllowedScopes.Any())
                    {
                        string scopeTableName = AttributeExtension.GetTableAttributeName<ClientScope>();
                        var scopeColumns = new[]
                        {
                            "Scope", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, scopeTableName, scopeColumns,
                            entity.AllowedScopes.Select(x => x.Scope));
                    }

                    if (entity.RedirectUris.Any())
                    {
                        string redirectUriTableName = AttributeExtension.GetTableAttributeName<ClientRedirectUri>();
                        var redirectUriColumns = new[]
                        {
                            "RedirectUri", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, redirectUriTableName, redirectUriColumns,
                            entity.RedirectUris.Select(x => x.RedirectUri));
                    }

                    if (entity.PostLogoutRedirectUris.Any())
                    {
                        string postLogoutRedirectUriTableName = AttributeExtension.GetTableAttributeName<ClientPostLogoutRedirectUri>();
                        var postLogoutRedirectUriColumns = new[]
                        {
                            "PostLogoutRedirectUri", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, postLogoutRedirectUriTableName,
                            postLogoutRedirectUriColumns,
                            entity.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri));
                    }

                    if (entity.IdentityProviderRestrictions.Any())
                    {
                        string identityProviderRestrictionsTableName = AttributeExtension.GetTableAttributeName<ClientIdPRestriction>();
                        var identityProviderRestrictionsColumns = new[]
                        {
                            "Provider", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, identityProviderRestrictionsTableName,
                            identityProviderRestrictionsColumns,
                            entity.IdentityProviderRestrictions.Select(x => x.Provider));
                    }

                    if (entity.AllowedCorsOrigins.Any())
                    {
                        string allowedCorsOriginsTableName = AttributeExtension.GetTableAttributeName<ClientCorsOrigin>();
                        var allowedCorsOriginsColumns = new[]
                        {
                            "Origin", "ClientId"
                        };
                        await UpdateItems(session, transaction, entity.Id, allowedCorsOriginsTableName,
                            allowedCorsOriginsColumns, entity.AllowedCorsOrigins.Select(x => x.Origin));
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                Log.Error($"ClientRepository >> UpdateAsync Error: {ex.Message}");

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }

        private async Task UpdateItems(IDbSession session, IDbTransaction transaction, int clientId, string tableName, IEnumerable<string> tableColumns, IEnumerable<string> columns)
        {
            var deleteQuery = new Query(tableName).Where("ClientId", "=", clientId).AsDelete();
            var deleteSqlResult = GetSqlResult(deleteQuery);
            await session.Connection.ExecuteAsync(deleteSqlResult.Sql, deleteSqlResult.NamedBindings, transaction);

            IEnumerable<string> enumerable = columns as string[] ?? columns.ToArray();
            if (enumerable.Any())
            {
                var updateDataList = enumerable.Select(x => new object[]
                {
                    x,
                    clientId
                });

                var insertQuery = new Query(tableName).AsInsert(tableColumns, updateDataList);
                var insertResult = GetSqlResult(insertQuery);

                await session.Connection.ExecuteAsync(insertResult.Sql, insertResult.NamedBindings, transaction);
            }

        }
    }
}
