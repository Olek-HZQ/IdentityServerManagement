using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Configuration;
using SqlKata;
using SqlKata.Compilers;

namespace IdentityServer.Admin.Dapper.Repositories.SqlServer
{
    public abstract class MssqlRepositoryBase<T> : RepositoryBase<T> where T : class
    {
        protected DbConnectionConfiguration DbConnectionConfig { get; }

        protected MssqlRepositoryBase(DbConnectionConfiguration dbConnectionConfig)
        {
            DbConnectionConfig = dbConnectionConfig;
        }

        protected sealed override DataProviderType DataProviderType => DataProviderType.SqlServer;

        protected override string ConnectionString => DbConnectionConfig.MasterSqlServerConnString;

        protected override SqlResult GetSqlResult(Query query, bool useLegacyPagination = true)
        {
            var compiler = new SqlServerCompiler();

            if (!useLegacyPagination)
            {
                compiler.UseLegacyPagination = false;
            }

            SqlResult sqlResult = compiler.Compile(query);

            return sqlResult;
        }
    }
}
