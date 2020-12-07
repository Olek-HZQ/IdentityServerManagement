using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Role;
using IdentityServer.Admin.Dapper.Repositories.Role;

namespace IdentityServer.Admin.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedRoleDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(search, page, pageSize);
        }

        public async Task<Core.Entities.Users.Role> GetRoleByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<List<Core.Entities.Users.Role>> GetAllRolesAsync()
        {
            return await _repository.GetAllRolesAsync();
        }

        public async Task<int> InsertRoleAsync(Core.Entities.Users.Role role)
        {
            return await _repository.InsertAsync(role);
        }

        public async Task<bool> UpdateRoleAsync(Core.Entities.Users.Role role)
        {
            return await _repository.UpdateAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(Core.Entities.Users.Role role)
        {
            return await _repository.DeleteAsync(role);
        }
    }
}
