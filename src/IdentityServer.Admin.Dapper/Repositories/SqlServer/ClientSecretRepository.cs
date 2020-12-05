using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.SqlServer
{
    public class ClientSecretRepository : MssqlRepositoryBase<ClientSecret>, IClientSecretRepository
    {
        public ClientSecretRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<PagedClientSecretDto> GetPagedAsync(int clientId, int page, int pageSize)
        {
            var clientSecret = new PagedClientSecretDto();

            if (clientId <= 0)
            {
                return clientSecret;
            }

            IDbSession session = DbSession;

            var clientQuery = new Query(AttributeExtension.GetTableAttributeName<Client>()).Select("Id", "ClientName").Where("Id", "=", clientId);
            var clientSqlResult = GetSqlResult(clientQuery);
            var client = await session.Connection.QueryFirstOrDefaultAsync<Client>(clientSqlResult.Sql, clientSqlResult.NamedBindings);

            try
            {
                if (client != null)
                {
                    clientSecret.ClientId = client.Id;
                    clientSecret.ClientName = client.ClientName;

                    var totalCountQuery = new Query(TableName).AsCount();
                    var resultQuery = new Query(TableName).Select("*");

                    totalCountQuery = totalCountQuery.Where("ClientId", "=", clientId);
                    resultQuery = resultQuery.Where("ClientId", "=", clientId);

                    var totalCountSqlResult = GetSqlResult(totalCountQuery);

                    resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                    var clientSecretSqlResult = GetSqlResult(resultQuery);

                    clientSecret.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                    clientSecret.DataPagedList = (await session.Connection.QueryAsync<ClientSecretDetailForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                    clientSecret.PageSize = pageSize;

                    return clientSecret;
                }
            }
            finally
            {
                session.Dispose();
            }

            return clientSecret;
        }
    }
}
