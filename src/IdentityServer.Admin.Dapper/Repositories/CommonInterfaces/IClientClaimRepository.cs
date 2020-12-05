using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IClientClaimRepository : IRepository<ClientClaim>
    {
        Task<PagedClientClaimDto> GetPagedAsync(int clientId, int page, int pageSize);
    }
}
