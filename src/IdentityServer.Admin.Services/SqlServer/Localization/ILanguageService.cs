using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Localization;

namespace IdentityServer.Admin.Services.SqlServer.Localization
{
    public interface ILanguageService
    {
        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false);

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
        Task InsertLanguageAsync(Language language);

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        Task UpdateLanguageAsync(Language language);

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        Task DeleteLanguageAsync(Language language);
    }
}
