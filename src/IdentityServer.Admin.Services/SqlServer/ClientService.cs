using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _repository.GetClientByIdAsync(id);
        }

        public async Task<Client> IsExistsAnyClientAsync()
        {
            return await _repository.IsExistsAnyClientAsync();
        }

        public PagedClientDto GetPagedClients(string search, int page, int pageSize = PageConstant.PageSize)
        {
            return _repository.GetPagedClients(search, page, pageSize);
        }

        public async Task<int> InsertClientAsync(Client client)
        {
            return await _repository.InsertAsync(client);
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            return await _repository.UpdateAsync(client);
        }

        public async Task<bool> DeleteClientAsync(Client client)
        {
            return await _repository.DeleteAsync(client);
        }
    }
}
