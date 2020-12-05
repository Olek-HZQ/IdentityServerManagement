using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IApiResourceService
    {
        Task<ApiResource> GetApiResourceByIdAsync(int id);

        Task<PagedApiResourceDto> GetPagedAsync(string name, int page = 1, int pageSize = PageConstant.PageSize);

        Task<int> InsertApiResourceAsync(ApiResource apiResource);

        Task<bool> UpdateApiResourceAsync(ApiResource apiResource);

        Task<bool> DeleteApiResourceAsync(ApiResource apiResource);
    }
}
