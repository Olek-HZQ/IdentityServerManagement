using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
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
