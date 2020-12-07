using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.IdentityResource;

namespace IdentityServer.Admin.Dapper.Repositories.IdentityResource
{
    public interface IIdentityResourceRepository : IRepository<Core.Entities.IdentityResource.IdentityResource>
    {
        Task<List<string>> GetResourcesAsync(string resource, int limit);

        Task<List<Core.Entities.IdentityResource.IdentityResource>> GetAllIdentityResourceList();

        Task<List<Core.Entities.IdentityResource.IdentityResource>> GetIdentityResourcesByScopeNameAsync(string[] scopeNames);

        Task<PagedIdentityResourceDto> GetPagedAsync(string name, int page, int pageSize);
    }
}
