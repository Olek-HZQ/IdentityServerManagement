using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Client;

namespace IdentityServer.Admin.Dapper.Repositories.Client
{
    public interface IClientRepository : IRepository<Core.Entities.Clients.Client>
    {
        Task<Core.Entities.Clients.Client> GetClientByIdAsync(int id);

        Task<Core.Entities.Clients.Client> IsExistsAnyClientAsync();

        Task<Core.Entities.Clients.Client> GetClientByClientId(string clientId);

        Task<List<string>> GetClientCorsOriginList(string origin);

        List<Core.Entities.Clients.Client> GetAllClientsAsync();

        PagedClientDto GetPagedClients(string search, int page, int pageSize);
    }
}
