using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IApiResourceScopeService
    {
        Task<ApiResourceScope> GetApiResourceScopeByIdAsync(int id);

        Task<PagedApiResourceScopeDto> GetPagedAsync(int apiResourceId, int page, int pageSize = PageConstant.PageSize);

        Task<int> InsertApiResourceScopeAsync(ApiResourceScope apiResourceScope);

        Task<bool> UpdateApiResourceScopeAsync(ApiResourceScope apiResourceScope);

        Task<bool> DeleteApiResourceScopeAsync(ApiResourceScope apiResourceScope);
    }
}
