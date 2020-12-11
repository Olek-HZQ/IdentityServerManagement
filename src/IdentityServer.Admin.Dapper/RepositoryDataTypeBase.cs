using System.Data;
using System.Data.SqlClient;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Entities.Enums;
using IdentityServer.Admin.Core.Extensions;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using SqlKata;
using SqlKata.Compilers;

namespace IdentityServer.Admin.Dapper
{
    public abstract class RepositoryDataTypeBase<T> where T : class
    {
        protected IDbSession DbSession => new DbSession(DbConnection());

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        protected virtual IDbConnection DbConnection()
        {
            IDbConnection conn;

            switch (DataProviderType)
            {
                case DataProviderType.Mysql:
                    conn = new MySqlConnection(ConnectionString);
                    break;

                case DataProviderType.Oracle:
                    conn = new OracleConnection(ConnectionString);
                    break;

                default:
                    conn = new SqlConnection(ConnectionString);
                    break;
            }

            conn.Open();

            return conn;
        }

        /// <summary>
        /// 数据库类型（SQL SERVER,MYSQL...）
        /// </summary>
        protected abstract DataProviderType DataProviderType { get; }

        protected abstract string ConnectionString { get; }

        /// <summary>
        /// 数据表名(默认类名或者TableAttribute配置属性值，如果不是，需要在子类重写)
        /// </summary>
        protected virtual string TableName => AttributeExtension.GetTableAttributeName<T>();

        /// <summary>
        /// 生成SQL语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="useLegacyPagination">目前这个只用于SqlServer, 标识是否使用Row_Number分页</param>
        /// <returns></returns>
        protected virtual SqlResult GetSqlResult(Query query, bool useLegacyPagination = false)
        {
            switch (DataProviderType)
            {
                case DataProviderType.Mysql:
                    return new MySqlCompiler().Compile(query);

                case DataProviderType.Oracle:
                    return new OracleCompiler().Compile(query);

                default:
                    var compiler = new SqlServerCompiler
                    {
                        UseLegacyPagination = useLegacyPagination
                    };

                    return compiler.Compile(query);
            }
        }
    }
}
