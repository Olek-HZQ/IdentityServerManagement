using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IRoleService
    {
        Task<PagedRoleDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize);

        Task<Role> GetRoleByIdAsync(int id);

        Task<List<Role>> GetAllRolesAsync();

        Task<int> InsertRoleAsync(Role role);

        Task<bool> UpdateRoleAsync(Role role);

        Task<bool> DeleteRoleAsync(Role role);
    }
}
