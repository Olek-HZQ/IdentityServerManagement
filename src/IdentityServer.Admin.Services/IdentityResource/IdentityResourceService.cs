using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.IdentityResource;
using IdentityServer.Admin.Dapper.Repositories.IdentityResource;

namespace IdentityServer.Admin.Services.IdentityResource
{
    public class IdentityResourceService : IIdentityResourceService
    {
        private readonly IIdentityResourceRepository _repository;

        public IdentityResourceService(IIdentityResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Core.Entities.IdentityResource.IdentityResource> GetIdentityResourceByIdAsync(int id)
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

        public async Task<int> InsertIdentityResourceAsync(Core.Entities.IdentityResource.IdentityResource identityResource)
        {
            return await _repository.InsertAsync(identityResource);
        }

        public async Task<bool> UpdateIdentityResourceAsync(Core.Entities.IdentityResource.IdentityResource identityResource)
        {
            return await _repository.UpdateAsync(identityResource);
        }

        public async Task<bool> DeleteIdentityResourceAsync(Core.Entities.IdentityResource.IdentityResource identityResource)
        {
            return await _repository.DeleteAsync(identityResource);
        }
    }
}
