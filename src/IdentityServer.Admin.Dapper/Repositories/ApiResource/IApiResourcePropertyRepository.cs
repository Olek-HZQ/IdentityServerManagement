using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Core.Entities.ApiResource;

namespace IdentityServer.Admin.Dapper.Repositories.ApiResource
{
    public interface IApiResourcePropertyRepository : IRepository<ApiResourceProperty>
    {
        Task<PagedApiResourcePropertyDto> GetPagedAsync(int apiResourceId, int page, int pageSize);
    }
}
