using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Configuration;
using SqlKata;
using SqlKata.Compilers;

namespace IdentityServer.Admin.Dapper.Repositories.Mysql
{
    public class MysqlRepositoryBase<T> : RepositoryBase<T> where T : class
    {
        protected DbConnectionConfiguration DbConnectionConfig { get; }

        protected MysqlRepositoryBase(DbConnectionConfiguration dbConnectionConfig)
        {
            DbConnectionConfig = dbConnectionConfig;
        }

        protected sealed override DataProviderType DataProviderType => DataProviderType.Mysql;

        protected override string ConnectionString => DbConnectionConfig.MasterMySqlConnString;

        protected override SqlResult GetSqlResult(Query query, bool useLegacyPagination = true)
        {
            var compiler = new MySqlCompiler();

            SqlResult sqlResult = compiler.Compile(query);

            return sqlResult;
        }
    }
}
