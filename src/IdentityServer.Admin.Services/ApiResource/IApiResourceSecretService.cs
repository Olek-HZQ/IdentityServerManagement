using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;

namespace IdentityServer.Admin.Services.ApiResource
{
    public interface IApiResourceSecretService
    {
        Task<ApiResourceSecret> GetApiResourceSecretById(int id);

        Task<PagedApiResourceSecretDto> GetPagedAsync(int apiResourceId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<int> InsertApiResourceSecret(ApiResourceSecret apiResourceSecret);

        Task<bool> UpdateApiResourceSecret(ApiResourceSecret apiResourceSecret);

        Task<bool> DeleteApiResourceSecret(ApiResourceSecret apiResourceSecret);
    }
}
