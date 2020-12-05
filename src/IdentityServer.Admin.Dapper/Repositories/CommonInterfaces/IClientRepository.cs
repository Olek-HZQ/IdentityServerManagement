using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetClientByIdAsync(int id);

        Task<Client> IsExistsAnyClientAsync();

        Task<Client> GetClientByClientId(string clientId);

        Task<List<string>> GetClientCorsOriginList(string origin);

        List<Client> GetAllClientsAsync();

        PagedClientDto GetPagedClients(string search, int page, int pageSize);
    }
}
