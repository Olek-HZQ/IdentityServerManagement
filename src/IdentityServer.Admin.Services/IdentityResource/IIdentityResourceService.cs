using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.IdentityResource;

namespace IdentityServer.Admin.Services.IdentityResource
{
    public interface IIdentityResourceService
    {
        Task<Core.Entities.IdentityResource.IdentityResource> GetIdentityResourceByIdAsync(int id);

        Task<List<string>> GetResourcesAsync(string resource, int limit = 0);

        Task<PagedIdentityResourceDto> GetPagedAsync(string name, int page, int pageSize = PageConstant.PageSize);

        Task<int> InsertIdentityResourceAsync(Core.Entities.IdentityResource.IdentityResource identityResource);

        Task<bool> UpdateIdentityResourceAsync(Core.Entities.IdentityResource.IdentityResource identityResource);

        Task<bool> DeleteIdentityResourceAsync(Core.Entities.IdentityResource.IdentityResource identityResource);
    }
}
