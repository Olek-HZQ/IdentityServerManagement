using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Dapper.Repositories.Client;

namespace IdentityServer.Admin.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Core.Entities.Clients.Client> GetClientByIdAsync(int id)
        {
            return await _repository.GetClientByIdAsync(id);
        }

        public async Task<Core.Entities.Clients.Client> IsExistsAnyClientAsync()
        {
            return await _repository.IsExistsAnyClientAsync();
        }

        public PagedClientDto GetPagedClients(string search, int page, int pageSize = PageConstant.PageSize)
        {
            return _repository.GetPagedClients(search, page, pageSize);
        }

        public async Task<int> InsertClientAsync(Core.Entities.Clients.Client client)
        {
            return await _repository.InsertAsync(client);
        }

        public async Task<bool> UpdateClientAsync(Core.Entities.Clients.Client client)
        {
            return await _repository.UpdateAsync(client);
        }

        public async Task<bool> DeleteClientAsync(Core.Entities.Clients.Client client)
        {
            return await _repository.DeleteAsync(client);
        }
    }
}
