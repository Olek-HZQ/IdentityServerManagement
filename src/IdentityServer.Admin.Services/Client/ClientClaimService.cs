using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Dapper.Repositories.Client;

namespace IdentityServer.Admin.Services.Client
{
    public class ClientClaimService : IClientClaimService
    {
        private readonly IClientClaimRepository _repository;

        public ClientClaimService(IClientClaimRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedClientClaimDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(clientId, page, pageSize);
        }

        public async Task<ClientClaim> GetClientClaimById(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _repository.GetAsync(id);
        }

        public async Task<int> InsertClientClaim(ClientClaim clientClaim)
        {
            return await _repository.InsertAsync(clientClaim);
        }

        public async Task<bool> DeleteClientClaim(ClientClaim clientClaim)
        {
            return await _repository.DeleteAsync(clientClaim);
        }
    }
}
