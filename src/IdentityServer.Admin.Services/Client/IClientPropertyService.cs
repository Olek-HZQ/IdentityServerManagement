using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;

namespace IdentityServer.Admin.Services.Client
{
    public interface IClientPropertyService
    {
        Task<PagedClientPropertyDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<ClientProperty> GetClientPropertyById(int id);

        Task<int> InsertClientPropertyAsync(ClientProperty clientProperty);

        Task<bool> DeleteClientPropertyAsync(ClientProperty clientProperty);
    }
}
