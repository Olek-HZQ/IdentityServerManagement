using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;

namespace IdentityServer.Admin.Services.Localization
{
    public interface ILanguageService
    {
        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false);

        Task<PagedLanguageDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Language</returns>
        Task<Language> GetLanguageByIdAsync(int languageId);

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        Task<int> InsertLanguageAsync(Language language);

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        Task<bool> UpdateLanguageAsync(Language language);

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        Task<bool> DeleteLanguageAsync(Language language);
    }
}
