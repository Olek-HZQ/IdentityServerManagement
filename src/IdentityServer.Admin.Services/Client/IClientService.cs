using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;

namespace IdentityServer.Admin.Services.Client
{
    public interface IClientService
    {
        Task<Core.Entities.Clients.Client> GetClientByIdAsync(int id);

        Task<Core.Entities.Clients.Client> IsExistsAnyClientAsync();

        PagedClientDto GetPagedClients(string search, int page, int pageSize = PageConstant.PageSize);

        Task<int> InsertClientAsync(Core.Entities.Clients.Client client);

        Task<bool> UpdateClientAsync(Core.Entities.Clients.Client client);

        Task<bool> DeleteClientAsync(Core.Entities.Clients.Client client);
    }
}
