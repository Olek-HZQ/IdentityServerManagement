﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Role;
using IdentityServer.Admin.Core.Extensions;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.Role
{
    public class RoleRepository : RepositoryBase<Core.Entities.Users.Role>, IRoleRepository
    {
        public RoleRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<PagedRoleDto> GetPagedAsync(string search, int page, int pageSize)
        {
            var result = new PagedRoleDto();

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
                result.DataPagedList = (await session.Connection.QueryAsync<Core.Entities.Users.Role>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<List<Core.Entities.Users.Role>> GetAllRolesAsync()
        {
            IDbSession session = DbSession;

            var query=new Query(AttributeExtension.GetTableAttributeName<Core.Entities.Users.Role>()).Select("*");
            var sqlResult = GetSqlResult(query);

            var roles = (await session.Connection.QueryAsync<Core.Entities.Users.Role>(sqlResult.Sql,sqlResult.NamedBindings)).ToList();

            session.Dispose();

            return roles;
        }
    }
}
