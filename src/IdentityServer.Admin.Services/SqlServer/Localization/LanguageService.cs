using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces.Localization;

namespace IdentityServer.Admin.Services.SqlServer.Localization
{
    public class LanguageService : ILanguageService
    {
        public LanguageService(ILanguageRepository repository)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false)
        {
            throw new NotImplementedException();
        }

        public Task<Language> GetLanguageByIdAsync(int languageId)
        {
            throw new NotImplementedException();
        }

        public Task InsertLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLanguageAsync(Language language)
        {
            throw new NotImplementedException();
        }
    }
}
