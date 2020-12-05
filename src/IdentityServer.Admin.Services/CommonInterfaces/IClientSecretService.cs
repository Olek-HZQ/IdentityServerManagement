using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IClientSecretService
    {
        Task<PagedClientSecretDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<ClientSecret> GetClientSecretById(int id);

        Task<int> InsertClientSecretAsync(ClientSecret clientSecret);

        Task<bool> DeleteClientSecretAsync(ClientSecret clientSecret);
    }
}
