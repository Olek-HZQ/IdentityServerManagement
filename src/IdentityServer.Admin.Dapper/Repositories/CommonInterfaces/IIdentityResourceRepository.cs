using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IIdentityResourceRepository : IRepository<IdentityResource>
    {
        Task<List<string>> GetResourcesAsync(string resource, int limit);

        Task<List<IdentityResource>> GetAllIdentityResourceList();

        Task<List<IdentityResource>> GetIdentityResourcesByScopeNameAsync(string[] scopeNames);

        Task<PagedIdentityResourceDto> GetPagedAsync(string name, int page, int pageSize);
    }
}
