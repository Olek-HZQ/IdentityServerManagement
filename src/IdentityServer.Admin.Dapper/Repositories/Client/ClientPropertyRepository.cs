using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Core.Extensions;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.Client
{
    public class ClientPropertyRepository : RepositoryBase<ClientProperty>, IClientPropertyRepository
    {
        public ClientPropertyRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<PagedClientPropertyDto> GetPagedAsync(int clientId, int page, int pageSize)
        {
            var result = new PagedClientPropertyDto();

            if (clientId <= 0)
            {
                return result;
            }

            IDbSession session = DbSession;

            var clientQuery = new Query(AttributeExtension.GetTableAttributeName<Core.Entities.Clients.Client>()).Select("Id", "ClientName").Where("Id", "=", clientId);
            var clientSqlResult = GetSqlResult(clientQuery);
            var client = await session.Connection.QueryFirstOrDefaultAsync<Core.Entities.Clients.Client>(clientSqlResult.Sql, clientSqlResult.NamedBindings);

            try
            {
                if (client != null)
                {
                    result.ClientId = client.Id;
                    result.ClientName = client.ClientName;

                    var totalCountQuery = new Query(TableName).AsCount();
                    var resultQuery = new Query(TableName).Select("*");

                    totalCountQuery = totalCountQuery.Where("ClientId", "=", clientId);
                    resultQuery = resultQuery.Where("ClientId", "=", clientId);

                    var totalCountSqlResult = GetSqlResult(totalCountQuery);

                    resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                    var clientSecretSqlResult = GetSqlResult(resultQuery);

                    result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                    result.DataPagedList = (await session.Connection.QueryAsync<ClientPropertyForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                    result.PageSize = pageSize;

                    return result;
                }
            }
            finally
            {
                session.Dispose();
            }

            return result;
        }
    }
}
