using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Configuration;
using SqlKata;
using SqlKata.Compilers;

namespace IdentityServer.Admin.Dapper.Repositories.Oracle
{
    public class OracleRepositoryBase<T> : RepositoryBase<T> where T : class
    {
        protected DbConnectionConfiguration DbConnectionConfig { get; }

        protected OracleRepositoryBase(DbConnectionConfiguration dbConnectionConfig)
        {
            DbConnectionConfig = dbConnectionConfig;
        }

        protected sealed override DataProviderType DataProviderType => DataProviderType.SqlServer;

        protected override string ConnectionString => DbConnectionConfig.MasterOracleConnString;

        protected override SqlResult GetSqlResult(Query query, bool useLegacyPagination = true)
        {
            var compiler = new OracleCompiler();

            SqlResult sqlResult = compiler.Compile(query);

            return sqlResult;
        }
    }
}
