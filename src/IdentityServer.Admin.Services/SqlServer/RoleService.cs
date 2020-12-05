using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
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

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _repository.GetAllRolesAsync();
        }

        public async Task<int> InsertRoleAsync(Role role)
        {
            return await _repository.InsertAsync(role);
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            return await _repository.UpdateAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(Role role)
        {
            return await _repository.DeleteAsync(role);
        }
    }
}
