using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Core.Extensions;
using Serilog;
using SqlKata;

namespace IdentityServer.Admin.Dapper.Repositories.Localization
{
    public class LocalizationRepository : RepositoryBase<LocaleStringResource>, ILocalizationRepository
    {
        public LocalizationRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }

        public async Task<PagedLocalStringResourceDto> GetPagedAsync(int languageId, string search, int page, int pageSize)
        {
            var result = new PagedLocalStringResourceDto();

            IDbSession session = DbSession;

            try
            {
                var languageQuery = new Query(AttributeExtension.GetTableAttributeName<Language>()).Select("Id", "Name").Where("Id", "=", languageId);
                var languageSqlResult = GetSqlResult(languageQuery);
                var language = await session.Connection.QueryFirstOrDefaultAsync<Language>(languageSqlResult.Sql, languageSqlResult.NamedBindings);

                if (language != null)
                {
                    result.LanguageId = language.Id;
                    result.LanguageName = language.Name;

                    var totalCountQuery = new Query(TableName).AsCount();
                    var resultQuery = new Query(TableName).Select("*");

                    if (languageId > 0)
                    {
                        totalCountQuery = totalCountQuery.Where("LanguageId", "=", languageId);
                        resultQuery = resultQuery.Where("LanguageId", "=", languageId);
                    }

                    if (!string.IsNullOrEmpty(search))
                    {
                        totalCountQuery = totalCountQuery.WhereContains("ResourceName", search).OrWhereContains("ResourceValue", search);
                        resultQuery = resultQuery.WhereContains("ResourceName", search).OrWhereContains("ResourceValue", search);
                    }

                    var totalCountSqlResult = GetSqlResult(totalCountQuery);

                    resultQuery = resultQuery.OrderByDesc("Id").Offset((page - 1) * pageSize).Limit(pageSize);
                    var clientSecretSqlResult = GetSqlResult(resultQuery);

                    result.TotalCount = await session.Connection.QueryFirstOrDefaultAsync<int>(totalCountSqlResult.Sql, totalCountSqlResult.NamedBindings);
                    result.DataPagedList = (await session.Connection.QueryAsync<LocalStringResourceForPage>(clientSecretSqlResult.Sql, clientSecretSqlResult.NamedBindings)).ToList();

                    result.PageSize = pageSize;
                }

                return result;
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task<LocaleStringResource> GetResourceAsync(string resourceKey, int languageId)
        {
            var query = new Query(TableName).Select("*");
            if (!string.IsNullOrEmpty(resourceKey))
            {
                query = query.Where("ResourceName", "=", resourceKey);
            }

            if (languageId > 0)
            {
                query = query.Where("LanguageId", "=", languageId);
            }

            var sqlResult = GetSqlResult(query);

            return await GetFirstOrDefaultAsync(sqlResult.Sql, sqlResult.NamedBindings);
        }

        public async Task<List<LocaleStringResource>> GetResourcesByLanguageIdAsync(int languageId)
        {
            var query = new Query(TableName).Select("*");
            if (languageId > 0)
            {
                query = query.Where("LanguageId", "=", languageId);
            }

            var sqlResult = GetSqlResult(query);

            return (await GetListAsync(sqlResult.Sql, sqlResult.NamedBindings)).ToList();
        }

        public async Task<bool> SaveResourcesAsync(int languageId, List<LocaleStringResource> insertList, List<LocaleStringResource> updateList)
        {

            IDbSession session = DbSession;
            session.BeginTrans();
            IDbTransaction transaction = session.Transaction;

            try
            {
                if (insertList.Any())
                {
                    var cols = new[]
                    {
                        "ResourceName", "ResourceValue","LanguageId"
                    };

                    var insertData = insertList.Select(x => new object[]
                    {
                        x.ResourceName,
                        x.ResourceValue,
                        languageId
                    });

                    var insertQuery = new Query(TableName).AsInsert(cols, insertData);
                    var insertSqlResult = GetSqlResult(insertQuery);

                    await session.Connection.ExecuteAsync(insertSqlResult.Sql,
                        insertSqlResult.NamedBindings, transaction);
                }

                if (updateList.Any())
                {
                    updateList.ForEach(x =>
                    {
                        var updateQuery = new Query(TableName).Where("Id", "=", x.Id).AsUpdate(new
                        {
                            x.ResourceValue
                        });
                        var updateSqlResult = GetSqlResult(updateQuery);
                        session.Connection.Execute(updateSqlResult.Sql, updateSqlResult.NamedBindings, transaction);
                    });
                }

                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error($"LocalizationRepository >> SaveResourcesAsync Error: {ex.Message}");

                return false;
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
