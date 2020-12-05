using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IIdentityResourceService
    {
        Task<IdentityResource> GetIdentityResourceByIdAsync(int id);

        Task<List<string>> GetResourcesAsync(string resource, int limit = 0);

        Task<PagedIdentityResourceDto> GetPagedAsync(string name, int page, int pageSize = PageConstant.PageSize);

        Task<int> InsertIdentityResourceAsync(IdentityResource identityResource);

        Task<bool> UpdateIdentityResourceAsync(IdentityResource identityResource);

        Task<bool> DeleteIdentityResourceAsync(IdentityResource identityResource);
    }
}
