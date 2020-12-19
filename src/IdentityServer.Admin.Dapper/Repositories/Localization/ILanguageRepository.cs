using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;

namespace IdentityServer.Admin.Dapper.Repositories.Localization
{
    public interface ILanguageRepository : IRepository<Language>
    {
        Task<PagedLanguageDto> GetPagedAsync(string search, int page, int pageSize);
    }
}
