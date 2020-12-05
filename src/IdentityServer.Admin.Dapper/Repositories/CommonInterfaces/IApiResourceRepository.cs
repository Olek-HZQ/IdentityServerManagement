using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IApiResourceRepository : IRepository<ApiResource>
    {
        Task<PagedApiResourceDto> GetPagedAsync(string name, int page, int pageSize);

        Task<List<ApiResource>> GetApiResourcesByNameAsync(string[] apiResourceNames);

        Task<List<ApiResource>> GetApiResourcesByScopeNameAsync(string[] scopeNames);
    }
}
