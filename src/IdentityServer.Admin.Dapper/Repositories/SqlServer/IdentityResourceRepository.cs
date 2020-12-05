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
    public class IdentityResourceRepository : MssqlRepositoryBase<IdentityResource>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public override async Task<IdentityResource> GetAsync(int id, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*").Where("Id", "=", id);
            var sqlResult = GetSqlResult(query);

            var identityResource = await session.Connection.QueryFirstOrDefaultAsync<IdentityResource>(sqlResult.Sql, sqlResult.NamedBindings);

            if (identityResource.Id > 0)
            {
                var claimsQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceClaim>()).Select("Type").Where("IdentityResourceId", "=", identityResource.Id);
                var claimsSqlResult = GetSqlResult(claimsQuery);
                identityResource.UserClaims = (await session.Connection.QueryAsync<IdentityResourceClaim>(claimsSqlResult.Sql, claimsSqlResult.NamedBindings)).ToList();
            }

            session.Dispose();

            return identityResource;
        }

        public async Task<List<string>> GetResourcesAsync(string resource, int limit)
        {
            var query = new Query(TableName).Select("Name").WhereTrue("Enabled");

            if (!string.IsNullOrEmpty(resource))
            {
                query = query.WhereContains("Name", resource);
            }

            if (limit > 0)
            {
                query = query.Limit(limit);
            }

            var sqlResult = GetSqlResult(query);

            return (await GetListAsync(sqlResult.Sql, sqlResult.NamedBindings)).Select(x => x.Name).ToList();
        }

        public async Task<List<IdentityResource>> GetAllIdentityResourceList()
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*");
            var sqlResult = GetSqlResult(query);

            var result = (await session.Connection.QueryAsync<IdentityResource>(sqlResult.Sql, sqlResult.NamedBindings)).ToList();

            if (result.Any())
            {
                var identityResourceIds = result.Select(x => x.Id).ToArray();

                var claimQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceClaim>()).Select("*").WhereIn("IdentityResourceId", identityResourceIds);
                var claimSqlResult = GetSqlResult(claimQuery);

                var propertyQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceProperty>()).Select("*").WhereIn("IdentityResourceId", identityResourceIds);
                var propertySqlResult = GetSqlResult(propertyQuery);

                var multi = await session.Connection.QueryMultipleAsync($"{claimSqlResult.Sql};{propertySqlResult.Sql}", claimSqlResult.NamedBindings);

                var claims = (await multi.ReadAsync<IdentityResourceClaim>()).ToList();
                var properties = (await multi.ReadAsync<IdentityResourceProperty>()).ToList();

                result.ForEach(x =>
                {
                    x.UserClaims = claims.Where(y => y.IdentityResourceId == x.Id).ToList();
                    x.Properties = properties.Where(y => y.IdentityResourceId == x.Id).ToList();
                });
            }

            return result;
        }

        public async Task<List<IdentityResource>> GetIdentityResourcesByScopeNameAsync(string[] scopeNames)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*");

            if (scopeNames != null && scopeNames.Any())
            {
                query = query.WhereIn("Name", scopeNames);
            }

            var sqlResult = GetSqlResult(query);

            var result = (await session.Connection.QueryAsync<IdentityResource>(sqlResult.Sql, sqlResult.NamedBindings)).ToList();

            if (result.Any())
            {
                var identityResourceIds = result.Select(x => x.Id).ToArray();

                var claimQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceClaim>()).Select("*").WhereIn("IdentityResourceId", identityResourceIds);
                var claimSqlResult = GetSqlResult(claimQuery);

                var propertyQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceProperty>()).Select("*").WhereIn("IdentityResourceId", identityResourceIds);
                var propertySqlResult = GetSqlResult(propertyQuery);

                var multi = await session.Connection.QueryMultipleAsync($"{claimSqlResult.Sql};{propertySqlResult.Sql}", claimSqlResult.NamedBindings);

                var claims = (await multi.ReadAsync<IdentityResourceClaim>()).ToList();
                var properties = (await multi.ReadAsync<IdentityResourceProperty>()).ToList();

                result.ForEach(x =>
                {
                    x.UserClaims = claims.Where(y => y.IdentityResourceId == x.Id).ToList();
                    x.Properties = properties.Where(y => y.IdentityResourceId == x.Id).ToList();
                });
            }

            return result;
        }

        public async Task<PagedIdentityResourceDto> GetPagedAsync(string name, int page, int pageSize)
        {
            var result = new PagedIdentityResourceDto();

            IDbSession session = DbSession;

            try
            {
                var totalCountQuery = new Query(TableName).AsCount();
                var resultQuery = new Query(TableName).Select("*");

                if (!string.IsNullOrEmpty(name))
                {
                    totalCountQuery = totalCountQuery.WhereContains("Name", name);
                    resultQuery = resultQuery.WhereContains("Name", name);
                }

                var totalCountSqlResult = GetSqlResult(totalCountQuery);

                resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                var clientSecretSqlResult = GetSqlResult(resultQuery);

                result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                result.DataPagedList = (await session.Connection.QueryAsync<IdentityResourceForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<int> InsertAsync(IdentityResource entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = session.BeginTrans();
            try
            {
                var insertResult = await session.Connection.InsertAsync(entity, transaction);

                if (insertResult > 0 && entity.UserClaims.Any())
                {
                    var claimsColumns = new[]
                    {
                        "Type", "IdentityResourceId"
                    };
                    var insertClaimsQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceClaim>())
                        .AsInsert(
                            claimsColumns, entity.UserClaims.Select(x => new object[]
                            {
                                x.Type,
                                insertResult
                            }));
                    var insertClaimsSqlResult = GetSqlResult(insertClaimsQuery);

                    await session.Connection.ExecuteAsync(insertClaimsSqlResult.Sql,
                        insertClaimsSqlResult.NamedBindings, transaction);
                }

                transaction.Commit();

                return insertResult;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                Log.Error($"IdentityResourceRepository >> InsertAsync Error: {ex.Message}");

                return 0;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<bool> UpdateAsync(IdentityResource entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;
            IDbTransaction transaction = session.BeginTrans();

            try
            {
                var updatedResult = await session.Connection.UpdateAsync(entity, transaction);

                if (updatedResult)
                {
                    var deleteQuery = new Query(AttributeExtension.GetTableAttributeName<IdentityResourceClaim>()).Where("IdentityResourceId", "=", entity.Id).AsDelete();
                    var deleteSqlResult = GetSqlResult(deleteQuery);

                    await session.Connection.ExecuteAsync(deleteSqlResult.Sql, deleteSqlResult.NamedBindings, transaction);

                    if (entity.UserClaims.Any())
                    {
                        var claimsColumns = new[]
                        {
                            "Type", "IdentityResourceId"
                        };
                        var insertClaimsQuery =
                            new Query(AttributeExtension.GetTableAttributeName<IdentityResourceClaim>()).AsInsert(
                                claimsColumns, entity.UserClaims.Select(x => new object[]
                                {
                                    x.Type,
                                    entity.Id
                                }));
                        var insertClaimsSqlResult = GetSqlResult(insertClaimsQuery);

                        await session.Connection.ExecuteAsync(insertClaimsSqlResult.Sql,
                            insertClaimsSqlResult.NamedBindings, transaction);
                    }
                }

                transaction.Commit();

                return updatedResult;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                Log.Error($"IdentityResourceRepository >> UpdateAsync Error: {ex.Message}");

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}