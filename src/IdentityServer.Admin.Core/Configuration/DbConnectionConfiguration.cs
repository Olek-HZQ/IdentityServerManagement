
using IdentityServer.Admin.Core.Entities.Enums;

namespace IdentityServer.Admin.Core.Configuration
{
    public class DbConnectionConfiguration
    {
        public DataProviderType CurrentDataProviderType { get; set; }

        public string MasterSqlServerConnString { get; set; }

        public string MasterMySqlConnString { get; set; }

        public string MasterOracleConnString { get; set; }
    }
}
