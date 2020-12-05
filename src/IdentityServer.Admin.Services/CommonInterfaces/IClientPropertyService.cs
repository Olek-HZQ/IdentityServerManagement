using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IClientPropertyService
    {
        Task<PagedClientPropertyDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize);

        Task<ClientProperty> GetClientPropertyById(int id);

        Task<int> InsertClientPropertyAsync(ClientProperty clientProperty);

        Task<bool> DeleteClientPropertyAsync(ClientProperty clientProperty);
    }
}
