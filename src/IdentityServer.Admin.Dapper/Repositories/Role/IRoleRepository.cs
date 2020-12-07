using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.Role;

namespace IdentityServer.Admin.Dapper.Repositories.Role
{
    public interface IRoleRepository : IRepository<Core.Entities.Users.Role>
    {
        Task<PagedRoleDto> GetPagedAsync(string search, int page, int pageSize);

        Task<List<Core.Entities.Users.Role>> GetAllRolesAsync();
    }
}
