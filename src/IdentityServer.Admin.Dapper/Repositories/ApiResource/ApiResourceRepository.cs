using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Core.Extensions;
using Serilog;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.ApiResource
{
    public class ApiResourceRepository : RepositoryBase<Core.Entities.ApiResource.ApiResource>, IApiResourceRepository
    {
        public ApiResourceRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public override async Task<Core.Entities.ApiResource.ApiResource> GetAsync(int id, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*").Where("Id", "=", id);
            var sqlResult = GetSqlResult(query);

            var apiResource = await session.Connection.QueryFirstOrDefaultAsync<Core.Entities.ApiResource.ApiResource>(sqlResult.Sql, sqlResult.NamedBindings);

            if (apiResource.Id > 0)
            {
                var claimsQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceClaim>()).Select("Type").Where("ApiResourceId", "=", apiResource.Id);
                var claimsSqlResult = GetSqlResult(claimsQuery);
                apiResource.UserClaims = (await session.Connection.QueryAsync<ApiResourceClaim>(claimsSqlResult.Sql, claimsSqlResult.NamedBindings)).ToList();
            }

            session.Dispose();

            return apiResource;
        }

        public async Task<PagedApiResourceDto> GetPagedAsync(string name, int page, int pageSize)
        {
            var result = new PagedApiResourceDto();

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
                result.DataPagedList = (await session.Connection.QueryAsync<ApiResourceForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<List<Core.Entities.ApiResource.ApiResource>> GetApiResourcesByNameAsync(string[] apiResourceNames)
        {
            IDbSession session = DbSession;

            var resultQuery = new Query(TableName).Select("*");

            if (apiResourceNames != null && apiResourceNames.Any())
            {
                resultQuery = resultQuery.WhereIn("Name", apiResourceNames);
            }

            var resultSqlResult = GetSqlResult(resultQuery);

            try
            {
                List<Core.Entities.ApiResource.ApiResource> result = (await session.Connection.QueryAsync<Core.Entities.ApiResource.ApiResource>(resultSqlResult.Sql, resultSqlResult.NamedBindings)).ToList();
                if (result.Any())
                {
                    var apiResourceIds = result.Select(x => x.Id).Distinct().ToArray();

                    var secretQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceSecret>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var secretSqlResult = GetSqlResult(secretQuery);
                    var secretResult = await session.Connection.QueryAsync<ApiResourceSecret>(secretSqlResult.Sql,
                            secretSqlResult.NamedBindings);

                    var scopeQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceScope>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var scopeSqlResult = GetSqlResult(scopeQuery);
                    var scopeResult =
                        await session.Connection.QueryAsync<ApiResourceScope>(scopeSqlResult.Sql,
                            scopeSqlResult.NamedBindings);

                    var userClaimQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceClaim>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var userClaimSqlResult = GetSqlResult(userClaimQuery);
                    var userClaimResult =
                        await session.Connection.QueryAsync<ApiResourceClaim>(userClaimSqlResult.Sql,
                            userClaimSqlResult.NamedBindings);

                    var propertyQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceProperty>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var propertySqlResult = GetSqlResult(propertyQuery);
                    var propertyvResult =
                        await session.Connection.QueryAsync<ApiResourceProperty>(propertySqlResult.Sql,
                            propertySqlResult.NamedBindings);


                    result.ForEach(x =>
                    {
                        x.Secrets = secretResult.Where(y => y.ApiResourceId == x.Id).ToList();
                        x.Scopes = scopeResult.Where(y => y.ApiResourceId == x.Id).ToList();
                        x.UserClaims = userClaimResult.Where(y => y.ApiResourceId == x.Id).ToList();
                        x.Properties = propertyvResult.Where(y => y.ApiResourceId == x.Id).ToList();
                    });
                }

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<List<Core.Entities.ApiResource.ApiResource>> GetApiResourcesByScopeNameAsync(string[] scopeNames)
        {
            IDbSession session = DbSession;

            var resultQuery = new Query($"{TableName} as t")
                .Join($"{AttributeExtension.GetTableAttributeName<ApiResourceScope>()} as t1", "t.Id", "t1.ApiResourceId")
                .Select("t.*");

            if (scopeNames != null && scopeNames.Any())
            {
                resultQuery = resultQuery.WhereIn("t1.Scope", scopeNames);
            }

            var resultSqlResult = GetSqlResult(resultQuery);

            try
            {
                List<Core.Entities.ApiResource.ApiResource> result = (await session.Connection.QueryAsync<Core.Entities.ApiResource.ApiResource>(resultSqlResult.Sql, resultSqlResult.NamedBindings)).ToList();
                if (result.Any())
                {
                    var apiResourceIds = result.Select(x => x.Id).Distinct().ToArray();

                    var secretQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceSecret>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var secretSqlResult = GetSqlResult(secretQuery);
                    var secretResult = await session.Connection.QueryAsync<ApiResourceSecret>(secretSqlResult.Sql,
                            secretSqlResult.NamedBindings);

                    var scopeQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceScope>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var scopeSqlResult = GetSqlResult(scopeQuery);
                    var scopeResult =
                        await session.Connection.QueryAsync<ApiResourceScope>(scopeSqlResult.Sql,
                            scopeSqlResult.NamedBindings);

                    var userClaimQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceClaim>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var userClaimSqlResult = GetSqlResult(userClaimQuery);
                    var userClaimResult =
                        await session.Connection.QueryAsync<ApiResourceClaim>(userClaimSqlResult.Sql,
                            userClaimSqlResult.NamedBindings);

                    var propertyQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceProperty>()).Select("*").WhereIn("ApiResourceId", apiResourceIds);
                    var propertySqlResult = GetSqlResult(propertyQuery);
                    var propertyvResult =
                        await session.Connection.QueryAsync<ApiResourceProperty>(propertySqlResult.Sql,
                            propertySqlResult.NamedBindings);


                    result.ForEach(x =>
                    {
                        x.Secrets = secretResult.Where(y => y.ApiResourceId == x.Id).ToList();
                        x.Scopes = scopeResult.Where(y => y.ApiResourceId == x.Id).ToList();
                        x.UserClaims = userClaimResult.Where(y => y.ApiResourceId == x.Id).ToList();
                        x.Properties = propertyvResult.Where(y => y.ApiResourceId == x.Id).ToList();
                    });
                }

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<int> InsertAsync(Core.Entities.ApiResource.ApiResource entity, bool useTransaction = false, int? commandTimeout = null)
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
                        "Type", "ApiResourceId"
                    };
                    var insertClaimsQuery =
                        new Query(AttributeExtension.GetTableAttributeName<ApiResourceClaim>()).AsInsert(
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

                Log.Error($"ApiResourceRepository >> InsertAsync Error: {ex.Message}");

                return 0;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<bool> UpdateAsync(Core.Entities.ApiResource.ApiResource entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;
            IDbTransaction transaction = session.BeginTrans();

            try
            {
                var updatedResult = await session.Connection.UpdateAsync(entity, transaction);

                if (updatedResult)
                {
                    var deleteQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceClaim>()).Where("ApiResourceId", "=", entity.Id).AsDelete();
                    var deleteSqlResult = GetSqlResult(deleteQuery);

                    await session.Connection.ExecuteAsync(deleteSqlResult.Sql, deleteSqlResult.NamedBindings, transaction);

                    if (entity.UserClaims.Any())
                    {
                        var claimsColumns = new[]
                        {
                            "Type", "ApiResourceId"
                        };
                        var insertClaimsQuery = new Query(AttributeExtension.GetTableAttributeName<ApiResourceClaim>())
                            .AsInsert(
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

                Log.Error($"ApiResourceRepository >> UpdateAsync Error: {ex.Message}");

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
