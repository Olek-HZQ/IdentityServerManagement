using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IApiScopeService
    {
        Task<List<string>> GetScopesAsync(string scope, int limit = 0);

        Task<PagedApiScopeDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        Task<ApiScope> GetApiScopeByIdAsync(int id);

        Task<int> InsertApiScopeAsync(ApiScope apiScope);

        Task<bool> UpdateApiScopeAsync(ApiScope apiScope);

        Task<bool> DeleteApiScopeAsync(ApiScope apiScope);
    }
}
