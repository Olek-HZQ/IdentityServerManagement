using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Client;
using IdentityServer.Admin.Core.Entities.Clients;

namespace IdentityServer.Admin.Dapper.Repositories.Client
{
    public interface IClientSecretRepository : IRepository<ClientSecret>
    {
        Task<PagedClientSecretDto> GetPagedAsync(int clientId, int page, int pageSize);
    }
}
