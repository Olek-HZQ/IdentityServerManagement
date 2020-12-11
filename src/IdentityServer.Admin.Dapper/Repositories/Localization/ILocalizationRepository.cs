using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;

namespace IdentityServer.Admin.Dapper.Repositories.Localization
{
    public interface ILocalizationRepository : IRepository<LocaleStringResource>
    {
        Task<PagedLocalStringResourceDto> GetPagedAsync(int languageId, string search, int page, int pageSize);

        Task<LocaleStringResource> GetResourceAsync(string resourceKey, int languageId);

        Task<List<LocaleStringResource>> GetResourcesByLanguageIdAsync(int languageId);

        Task<bool> SaveResourcesAsync(int languageId, List<LocaleStringResource> insertList, List<LocaleStringResource> updateList);
    }
}
