using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;
using IdentityServer.Admin.Core.Extensions;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.ApiResource
{
    public class ApiResourcePropertyRepository : RepositoryBase<ApiResourceProperty>, IApiResourcePropertyRepository
    {
        public ApiResourcePropertyRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public override async Task<ApiResourceProperty> GetAsync(int id, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            var query = new Query($"{TableName} as t")
                .Join($"{AttributeExtension.GetTableAttributeName<Core.Entities.ApiResource.ApiResource>()} as t1", "t.ApiResourceId", "t1.Id")
                .Select("t.*", "t1.Name as ApiResourceName").Where("t.Id", "=", id);
            var sqlResult = GetSqlResult(query);

            var apiScope = await session.Connection.QueryFirstOrDefaultAsync<ApiResourceProperty>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return apiScope;
        }

        public async Task<PagedApiResourcePropertyDto> GetPagedAsync(int apiResourceId, int page, int pageSize)
        {
            var result = new PagedApiResourcePropertyDto();

            IDbSession session = DbSession;

            var apiResourceQuery = new Query(AttributeExtension.GetTableAttributeName<Core.Entities.ApiResource.ApiResource>()).Select("Id", "Name").Where("Id", "=", apiResourceId);
            var apiResourceSqlResult = GetSqlResult(apiResourceQuery);
            var apiResource = await session.Connection.QueryFirstOrDefaultAsync<Core.Entities.ApiResource.ApiResource>(apiResourceSqlResult.Sql, apiResourceSqlResult.NamedBindings);

            try
            {
                if (apiResource != null)
                {
                    result.ApiResourceId = apiResource.Id;
                    result.ApiResourceName = apiResource.Name;

                    var totalCountQuery = new Query(TableName).AsCount();
                    var resultQuery = new Query(TableName).Select("*");

                    if (apiResourceId > 0)
                    {
                        totalCountQuery = totalCountQuery.Where("ApiResourceId", "=", apiResourceId);
                        resultQuery = resultQuery.Where("ApiResourceId", "=", apiResourceId);
                    }

                    var totalCountSqlResult = GetSqlResult(totalCountQuery);

                    resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                    var clientSecretSqlResult = GetSqlResult(resultQuery);

                    result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql,
                        totalCountSqlResult.NamedBindings);
                    result.DataPagedList =
                        (await session.Connection.QueryAsync<ApiResourcePropertyForPage>(clientSecretSqlResult.Sql,
                            clientSecretSqlResult.NamedBindings)).ToList();

                    result.PageSize = pageSize;
                }

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
