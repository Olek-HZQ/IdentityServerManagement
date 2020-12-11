using System.Threading.Tasks;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Entities.Common;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.Common
{
    public class GenericAttributeRepository : RepositoryBase<GenericAttribute>, IGenericAttributeRepository
    {
        public GenericAttributeRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<GenericAttribute> GetAttributeAsync(string keyGroup, string key, int entityId)
        {
            var query = new Query(TableName).Select("*");
            if (!string.IsNullOrEmpty(keyGroup))
            {
                query = query.Where("KeyGroup", "=", keyGroup);
            }

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where("Key", "=", key);
            }

            if (entityId > 0)
            {
                query = query.Where("EntityId", "=", entityId);
            }

            var sqlResult = GetSqlResult(query);

            return await GetFirstOrDefaultAsync(sqlResult.Sql, sqlResult.NamedBindings);
        }
    }
}
