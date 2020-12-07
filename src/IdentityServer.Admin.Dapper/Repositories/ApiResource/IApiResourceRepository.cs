using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiResource;

namespace IdentityServer.Admin.Dapper.Repositories.ApiResource
{
    public interface IApiResourceRepository : IRepository<Core.Entities.ApiResource.ApiResource>
    {
        Task<PagedApiResourceDto> GetPagedAsync(string name, int page, int pageSize);

        Task<List<Core.Entities.ApiResource.ApiResource>> GetApiResourcesByNameAsync(string[] apiResourceNames);

        Task<List<Core.Entities.ApiResource.ApiResource>> GetApiResourcesByScopeNameAsync(string[] scopeNames);
    }
}
