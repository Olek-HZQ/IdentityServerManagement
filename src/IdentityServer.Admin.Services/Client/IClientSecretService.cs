using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;

namespace IdentityServer.Admin.Services.Client
{
    public interface IClientSecretService
    {
        Task<PagedClientSecretDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<ClientSecret> GetClientSecretById(int id);

        Task<int> InsertClientSecretAsync(ClientSecret clientSecret);

        Task<bool> DeleteClientSecretAsync(ClientSecret clientSecret);
    }
}
