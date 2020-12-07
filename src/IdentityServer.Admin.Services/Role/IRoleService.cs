using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Role;

namespace IdentityServer.Admin.Services.Role
{
    public interface IRoleService
    {
        Task<PagedRoleDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        Task<Core.Entities.Users.Role> GetRoleByIdAsync(int id);

        Task<List<Core.Entities.Users.Role>> GetAllRolesAsync();

        Task<int> InsertRoleAsync(Core.Entities.Users.Role role);

        Task<bool> UpdateRoleAsync(Core.Entities.Users.Role role);

        Task<bool> DeleteRoleAsync(Core.Entities.Users.Role role);
    }
}
