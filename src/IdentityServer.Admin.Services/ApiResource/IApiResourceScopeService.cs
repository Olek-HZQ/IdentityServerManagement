using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;

namespace IdentityServer.Admin.Services.ApiResource
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
