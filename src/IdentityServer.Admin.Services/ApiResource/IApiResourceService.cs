using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiResource;

namespace IdentityServer.Admin.Services.ApiResource
{
    public interface IApiResourceService
    {
        Task<Core.Entities.ApiResource.ApiResource> GetApiResourceByIdAsync(int id);

        Task<PagedApiResourceDto> GetPagedAsync(string name, int page = 1, int pageSize = PageConstant.PageSize);

        Task<int> InsertApiResourceAsync(Core.Entities.ApiResource.ApiResource apiResource);

        Task<bool> UpdateApiResourceAsync(Core.Entities.ApiResource.ApiResource apiResource);

        Task<bool> DeleteApiResourceAsync(Core.Entities.ApiResource.ApiResource apiResource);
    }
}
