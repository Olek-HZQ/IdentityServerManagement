using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Dapper.Repositories.Localization;

namespace IdentityServer.Admin.Services.Localization
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _repository;

        public LanguageService(ILanguageRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false)
        {
            var languages = (await _repository.GetAllAsync()).ToList();

            if (!showHidden)
            {
                languages = languages.Where(x => x.Published).ToList();
            }

            return languages;
        }

        public async Task<PagedLanguageDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(search, page, pageSize);
        }

        public async Task<Language> GetLanguageByIdAsync(int languageId)
        {
            return await _repository.GetAsync(languageId);
        }

        public async Task<int> InsertLanguageAsync(Language language)
        {
            return await _repository.InsertAsync(language);
        }

        public async Task<bool> UpdateLanguageAsync(Language language)
        {
            return await _repository.UpdateAsync(language);
        }

        public async Task<bool> DeleteLanguageAsync(Language language)
        {
            return await _repository.DeleteAsync(language);
        }
    }
}
