using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
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
