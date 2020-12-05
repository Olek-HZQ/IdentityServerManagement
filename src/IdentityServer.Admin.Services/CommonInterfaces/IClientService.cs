using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IClientService
    {
        Task<Client> GetClientByIdAsync(int id);

        Task<Client> IsExistsAnyClientAsync();

        PagedClientDto GetPagedClients(string search, int page, int pageSize = PageConstant.PageSize);

        Task<int> InsertClientAsync(Client client);

        Task<bool> UpdateClientAsync(Client client);

        Task<bool> DeleteClientAsync(Client client);
    }
}
