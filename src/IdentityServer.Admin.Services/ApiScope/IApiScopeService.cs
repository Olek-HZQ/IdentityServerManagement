using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiScope;

namespace IdentityServer.Admin.Services.ApiScope
{
    public interface IApiScopeService
    {
        Task<List<string>> GetScopesAsync(string scope, int limit = 0);

        Task<PagedApiScopeDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        Task<Core.Entities.ApiScope.ApiScope> GetApiScopeByIdAsync(int id);

        Task<int> InsertApiScopeAsync(Core.Entities.ApiScope.ApiScope apiScope);

        Task<bool> UpdateApiScopeAsync(Core.Entities.ApiScope.ApiScope apiScope);

        Task<bool> DeleteApiScopeAsync(Core.Entities.ApiScope.ApiScope apiScope);
    }
}
