using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Dapper.Repositories.Client;

namespace IdentityServer.Admin.Services.Client
{
    public class ClientPropertyService : IClientPropertyService
    {
        private readonly IClientPropertyRepository _repository;

        public ClientPropertyService(IClientPropertyRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedClientPropertyDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(clientId, page, pageSize);
        }

        public async Task<ClientProperty> GetClientPropertyById(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);

        }

        public async Task<int> InsertClientPropertyAsync(ClientProperty clientProperty)
        {
            return await _repository.InsertAsync(clientProperty);
        }

        public async Task<bool> DeleteClientPropertyAsync(ClientProperty clientProperty)
        {
            return await _repository.DeleteAsync(clientProperty);
        }
    }
}
