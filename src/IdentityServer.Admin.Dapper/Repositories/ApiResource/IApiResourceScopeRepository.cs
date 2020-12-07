using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;

namespace IdentityServer.Admin.Dapper.Repositories.ApiResource
{
    public interface IApiResourceScopeRepository : IRepository<ApiResourceScope>
    {
        Task<PagedApiResourceScopeDto> GetPagedAsync(int apiResourceId, int page, int pageSize);
    }
}
