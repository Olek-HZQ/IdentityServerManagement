using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IApiResourceScopeRepository : IRepository<ApiResourceScope>
    {
        Task<PagedApiResourceScopeDto> GetPagedAsync(int apiResourceId, int page, int pageSize);
    }
}
