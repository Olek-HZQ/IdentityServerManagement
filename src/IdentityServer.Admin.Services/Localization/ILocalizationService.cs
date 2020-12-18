using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;

namespace IdentityServer.Admin.Services.Localization
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        Task<Dictionary<string, KeyValuePair<int, string>>> GetAllResourceValuesAsync(int languageId);

        Task<PagedLocalStringResourceDto> GetPagedAsync(int languageId, string search, int page, int pageSize = PageConstant.PageSize);

        Task<LocaleStringResource> GetStringResourceByIdAsync(int id);

        Task<string> GetResourceAsync(string resourceKey, bool logIfNotFound = true, string defaultValue = "");

        Task<int> InsertStringResourceAsync(LocaleStringResource resource);

        Task<bool> UpdateStringResourceAsync(LocaleStringResource resource);

        Task<bool> DeleteStringResourceAsync(LocaleStringResource resource);

        Task<bool> SaveResourcesAsync(int languageId, string data);
    }
}
