using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiScope;
using IdentityServer.Admin.Core.Entities.ApiScope;
using IdentityServer.Admin.Core.Extensions;
using Serilog;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.ApiScope
{
    public class ApiScopeRepository : RepositoryBase<Core.Entities.ApiScope.ApiScope>, IApiScopeRepository
    {
        public ApiScopeRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<List<string>> GetScopesAsync(string scope, int limit)
        {
            var query = new Query(TableName).Select("Name");

            if (!string.IsNullOrEmpty(scope))
            {
                query = query.WhereContains("Name", scope);
            }

            if (limit > 0)
            {
                query = query.Limit(limit);
            }

            var sqlResult = GetSqlResult(query);

            return (await GetListAsync(sqlResult.Sql, sqlResult.NamedBindings)).Select(x => x.Name).ToList();
        }

        public async Task<List<Core.Entities.ApiScope.ApiScope>> GetAllApiScopeListAsync()
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*");
            var sqlResult = GetSqlResult(query);

            var result = (await session.Connection.QueryAsync<Core.Entities.ApiScope.ApiScope>(sqlResult.Sql, sqlResult.NamedBindings)).ToList();

            if (result.Any())
            {
                var identityResourceIds = result.Select(x => x.Id).ToArray();

                var claimQuery = new Query(AttributeExtension.GetTableAttributeName<ApiScopeClaim>()).Select("*").WhereIn("ScopeId", identityResourceIds);
                var claimSqlResult = GetSqlResult(claimQuery);

                var claims = await session.Connection.QueryAsync<ApiScopeClaim>(claimSqlResult.Sql, claimSqlResult.NamedBindings);

                result.ForEach(x =>
                {
                    x.UserClaims = claims.Where(y => y.ScopeId == x.Id).ToList();
                });
            }

            return result;
        }

        public async Task<List<Core.Entities.ApiScope.ApiScope>> GetApiScopesByNameAsync(string[] scopeNames)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*");

            if (scopeNames != null && scopeNames.Any())
            {
                query = query.WhereIn("Name", scopeNames);
            }

            var sqlResult = GetSqlResult(query);

            var result = (await session.Connection.QueryAsync<Core.Entities.ApiScope.ApiScope>(sqlResult.Sql, sqlResult.NamedBindings)).ToList();

            if (result.Any())
            {
                var identityResourceIds = result.Select(x => x.Id).ToArray();

                var claimQuery = new Query(AttributeExtension.GetTableAttributeName<ApiScopeClaim>()).Select("*").WhereIn("ScopeId", identityResourceIds);
                var claimSqlResult = GetSqlResult(claimQuery);

                var claims = await session.Connection.QueryAsync<ApiScopeClaim>(claimSqlResult.Sql, claimSqlResult.NamedBindings);

                result.ForEach(x =>
                {
                    x.UserClaims = claims.Where(y => y.ScopeId == x.Id).ToList();
                });
            }

            session.Dispose();

            return result;
        }

        public override async Task<Core.Entities.ApiScope.ApiScope> GetAsync(int id, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Select("*").Where("Id", "=", id);
            var sqlResult = GetSqlResult(query);

            var apiScope = await session.Connection.QueryFirstOrDefaultAsync<Core.Entities.ApiScope.ApiScope>(sqlResult.Sql, sqlResult.NamedBindings);

            if (apiScope.Id > 0)
            {
                var claimsQuery = new Query(AttributeExtension.GetTableAttributeName<ApiScopeClaim>()).Select("Type").Where("ScopeId", "=", apiScope.Id);
                var claimsSqlResult = GetSqlResult(claimsQuery);
                apiScope.UserClaims = (await session.Connection.QueryAsync<ApiScopeClaim>(claimsSqlResult.Sql, claimsSqlResult.NamedBindings)).ToList();
            }

            session.Dispose();

            return apiScope;
        }

        public async Task<PagedApiScopeDto> GetPagedAsync(string search, int page, int pageSize)
        {
            var result = new PagedApiScopeDto();

            IDbSession session = DbSession;

            try
            {
                var totalCountQuery = new Query(TableName).AsCount();
                var resultQuery = new Query(TableName).Select("*");

                if (!string.IsNullOrEmpty(search))
                {
                    totalCountQuery = totalCountQuery.WhereContains("Name", search);
                    resultQuery = resultQuery.WhereContains("Name", search);
                }

                var totalCountSqlResult = GetSqlResult(totalCountQuery);

                resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                var clientSecretSqlResult = GetSqlResult(resultQuery);

                result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                result.DataPagedList = (await session.Connection.QueryAsync<ApiScopeForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<int> InsertAsync(Core.Entities.ApiScope.ApiScope entity, bool useTransaction = false, int? commandTimeout = null)
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
                        "Type", "ScopeId"
                    };
                    var insertClaimsQuery =
                        new Query(AttributeExtension.GetTableAttributeName<ApiScopeClaim>()).AsInsert(
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

                Log.Error($"ApiScopeRepository >> InsertAsync Error: {ex.Message}");

                return 0;
            }
            finally
            {
                session.Dispose();
            }
        }

        public override async Task<bool> UpdateAsync(Core.Entities.ApiScope.ApiScope entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;
            IDbTransaction transaction = session.BeginTrans();

            try
            {
                var updatedResult = await session.Connection.UpdateAsync(entity, transaction);

                if (updatedResult)
                {
                    var deleteQuery = new Query(AttributeExtension.GetTableAttributeName<ApiScopeClaim>()).Where("ScopeId", "=", entity.Id).AsDelete();
                    var deleteSqlResult = GetSqlResult(deleteQuery);

                    var deletedResult = await session.Connection.ExecuteAsync(deleteSqlResult.Sql, deleteSqlResult.NamedBindings, transaction);

                    if (deletedResult > 0 && entity.UserClaims.Any())
                    {
                        var claimsColumns = new[]
                        {
                            "Type", "ScopeId"
                        };
                        var insertClaimsQuery =
                            new Query(AttributeExtension.GetTableAttributeName<ApiScopeClaim>()).AsInsert(
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

                Log.Error($"ApiScopeRepository >> UpdateAsync Error: {ex.Message}");

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
