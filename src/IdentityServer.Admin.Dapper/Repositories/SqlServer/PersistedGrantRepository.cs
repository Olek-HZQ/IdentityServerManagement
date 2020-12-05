using System.Collections.Generic;
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
    public class PersistedGrantRepository : MssqlRepositoryBase<PersistedGrant>, IPersistedGrantRepository
    {
        public PersistedGrantRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<PagedPersistedGrantDto> GetPagedAsync(string search, int page, int pageSize)
        {
            var result = new PagedPersistedGrantDto();

            IDbSession session = DbSession;

            try
            {

                //var subjectIdQuery = new Query($"{TableName} as p")
                //    .LeftJoin($"{AttributeExtension.GetTableAttributeName<User>()} as u", "p.SubjectId", "u.SubjectId")
                //    .Select("p.SubjectId")
                //    .GroupBy("p.SubjectId");
                //var subjectIdSqlResult = GetSqlResult(subjectIdQuery);

                //var subjectIds = (await session.Connection.QueryAsync<string>(subjectIdSqlResult.Sql, subjectIdSqlResult.NamedBindings)).ToList();

                //if (!subjectIds.Any())
                //{
                //    return new PagedPersistedGrantDto();
                //}

                // string subjectIdString = string.Join(",", subjectIds);
                //var totalCountQuery = new Query().FromRaw($"STRING_SPLIT('{subjectIdString}',',')")
                //    .Join($"{TableName} as p", "value", "p.SubjectId")
                //    .LeftJoin($"{AttributeExtension.GetTableAttributeName<User>()} as u", "p.SubjectId", "u.SubjectId")
                //    .AsCount();
                //var resultQuery = new Query().FromRaw($"STRING_SPLIT('{subjectIdString}',',')")
                //    .Join($"{TableName} as p", "value", "p.SubjectId")
                //    .LeftJoin($"{AttributeExtension.GetTableAttributeName<User>()} as u", "p.SubjectId", "u.SubjectId")
                //    .Select("p.*", "u.Name as SubjectName");

                var withQuery = new Query($"{TableName} as p")
                    .LeftJoin($"{AttributeExtension.GetTableAttributeName<User>()} as u", "p.SubjectId", "u.SubjectId")
                    .Select("p.*", "u.Name as SubjectName")
                    .SelectRaw("ROW_NUMBER() OVER (PARTITION BY p.ClientId ORDER BY p.Expiration DESC) AS ROW_NUMBER");

                var totalCountQuery = new Query("Grants").With("Grants", withQuery).Where("Grants.ROW_NUMBER", "=", 1).AsCount();
                var resultQuery = new Query("Grants").With("Grants", withQuery).Where("Grants.ROW_NUMBER", "=", 1).Select("*");

                if (!string.IsNullOrEmpty(search))
                {
                    totalCountQuery = totalCountQuery.WhereContains("p.SubjectId", search).OrWhereContains("u.Name", search);
                    resultQuery = resultQuery.WhereContains("p.SubjectId", search).OrWhereContains("u.Name", search);
                }

                var totalCountSqlResult = GetSqlResult(totalCountQuery);

                resultQuery = resultQuery.OrderByDesc("Expiration").Offset((page - 1) * pageSize).Limit(pageSize);
                var clientSecretSqlResult = GetSqlResult(resultQuery);

                result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                result.DataPagedList = (await session.Connection.QueryAsync<PersistedGrantForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<PagedPersistedGrantByUserDto> GetPagedByUserAsync(string subjectId, int page, int pageSize)
        {
            var result = new PagedPersistedGrantByUserDto();

            IDbSession session = DbSession;

            try
            {
                var totalCountQuery = new Query($"{TableName} as p")
                    .Join($"{AttributeExtension.GetTableAttributeName<User>()} as u", "p.SubjectId", "u.SubjectId").AsCount();
                var resultQuery = new Query($"{TableName} as p")
                    .Join($"{AttributeExtension.GetTableAttributeName<User>()} as u", "p.SubjectId", "u.SubjectId")
                    .Select("p.*", "u.Name as SubjectName");

                if (!string.IsNullOrEmpty(subjectId))
                {
                    totalCountQuery = totalCountQuery.Where("p.SubjectId", "=", subjectId);
                    resultQuery = resultQuery.Where("p.SubjectId", "=", subjectId);
                }

                var totalCountSqlResult = GetSqlResult(totalCountQuery);

                resultQuery = resultQuery.OrderByDesc("p.Expiration").Offset((page - 1) * pageSize).Limit(pageSize);
                var clientSecretSqlResult = GetSqlResult(resultQuery);

                result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                result.DataPagedList = (await session.Connection.QueryAsync<PersistedGrantByUserForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                result.PageSize = pageSize;

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<PersistedGrant> GetByKeyAsync(string key)
        {
            IDbSession session = DbSession;

            var query = new Query(TableName).Where("Key", "=", key);

            var sqlResult = GetSqlResult(query);

            var result = await session.Connection.QueryFirstOrDefaultAsync<PersistedGrant>(sqlResult.Sql, sqlResult.NamedBindings);

            session.Dispose();

            return result;
        }

        public async Task<IEnumerable<PersistedGrant>> GetListBySubjectIdAsync(string subjectId)
        {
            var query = new Query(TableName).Where("SubjectId", "=", subjectId);

            var sqlResult = GetSqlResult(query);

            return await GetListAsync(sqlResult.Sql, sqlResult.NamedBindings);
        }

        public async Task<bool> DeleteByKeyAsync(string key)
        {
            var query = new Query(TableName).Where("Key", "=", key).AsDelete();

            var sqlResult = GetSqlResult(query);

            return await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings) > 0;
        }

        public async Task<bool> DeleteAllBySubjectIdAsync(string subjectId)
        {
            var query = new Query(TableName).Where("SubjectId", "=", subjectId).AsDelete();

            var sqlResult = GetSqlResult(query);

            return await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings) > 0;
        }

        public override async Task<int> InsertAsync(PersistedGrant entity, bool useTransaction = false, int? commandTimeout = null)
        {
            var query = new Query(TableName).AsInsert(new
            {
                entity.Key,
                entity.Type,
                entity.SubjectId,
                entity.SessionId,
                entity.ClientId,
                entity.Description,
                entity.CreationTime,
                entity.Expiration,
                entity.ConsumedTime,
                entity.Data
            });
            var sqlResult = GetSqlResult(query);

            LogSqlQueryExtensions.LogQuerySql("PersistedGrantRepository >> InsertAsync", sqlResult);

            return await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings);
        }

        public override async Task<bool> UpdateAsync(PersistedGrant entity, bool useTransaction = false, int? commandTimeout = null)
        {
            var query = new Query(TableName).Where("Key", "=", entity.Key).AsUpdate(new
            {
                entity.Key,
                entity.Type,
                entity.SubjectId,
                entity.SessionId,
                entity.ClientId,
                entity.Description,
                entity.CreationTime,
                entity.Expiration,
                entity.ConsumedTime,
                entity.Data
            });
            var sqlResult = GetSqlResult(query);

            LogSqlQueryExtensions.LogQuerySql("PersistedGrantRepository >> UpdateAsync", sqlResult);

            return await ExecuteAsync(sqlResult.Sql, sqlResult.NamedBindings) > 0;
        }
    }
}
