using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
{
    public class IdentityResourceService : IIdentityResourceService
    {
        private readonly IIdentityResourceRepository _repository;

        public IdentityResourceService(IIdentityResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IdentityResource> GetIdentityResourceByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public Task<List<string>> GetResourcesAsync(string resource, int limit = 0)
        {
            return _repository.GetResourcesAsync(resource, limit);
        }

        public async Task<PagedIdentityResourceDto> GetPagedAsync(string name, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(name, page, pageSize);
        }

        public async Task<int> InsertIdentityResourceAsync(IdentityResource identityResource)
        {
            return await _repository.InsertAsync(identityResource);
        }

        public async Task<bool> UpdateIdentityResourceAsync(IdentityResource identityResource)
        {
            return await _repository.UpdateAsync(identityResource);
        }

        public async Task<bool> DeleteIdentityResourceAsync(IdentityResource identityResource)
        {
            return await _repository.DeleteAsync(identityResource);
        }
    }
}
