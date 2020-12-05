using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<PagedRoleDto> GetPagedAsync(string search, int page, int pageSize);

        Task<List<Role>> GetAllRolesAsync();
    }
}
