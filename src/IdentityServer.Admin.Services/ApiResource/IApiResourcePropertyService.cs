using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;

namespace IdentityServer.Admin.Services.ApiResource
{
    public interface IApiResourcePropertyService
    {
        Task<ApiResourceProperty> GetApiResourcePropertyById(int id);

        Task<PagedApiResourcePropertyDto> GetPagedAsync(int apiResourceId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<int> InsertApiResourceProperty(ApiResourceProperty apiResourceProperty);

        Task<bool> UpdateApiResourceProperty(ApiResourceProperty apiResourceProperty);

        Task<bool> DeleteApiResourceProperty(ApiResourceProperty apiResourceProperty);
    }
}
