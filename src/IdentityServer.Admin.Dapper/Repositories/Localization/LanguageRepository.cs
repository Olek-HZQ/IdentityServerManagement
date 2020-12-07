using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces.Localization;
using IdentityServer.Admin.Dapper.Repositories.SqlServer;

namespace IdentityServer.Admin.Dapper.Repositories.Localization
{
    public class LanguageRepository : MssqlRepositoryBase<Language>, ILanguageRepository
    {
        public LanguageRepository(DbConnectionConfiguration dbConnectionConfig) : base(dbConnectionConfig)
        {
        }
    }
}
