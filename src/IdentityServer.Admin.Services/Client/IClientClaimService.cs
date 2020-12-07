using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;

namespace IdentityServer.Admin.Services.Client
{
    public interface IClientClaimService
    {
        Task<PagedClientClaimDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<ClientClaim> GetClientClaimById(int id);

        Task<int> InsertClientClaim(ClientClaim clientClaim);

        Task<bool> DeleteClientClaim(ClientClaim clientClaim);
    }
}
