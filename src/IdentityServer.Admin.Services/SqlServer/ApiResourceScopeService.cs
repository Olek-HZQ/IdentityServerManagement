using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
{
    public class ApiResourceScopeService : IApiResourceScopeService
    {
        private readonly IApiResourceScopeRepository _repository;

        public ApiResourceScopeService(IApiResourceScopeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResourceScope> GetApiResourceScopeByIdAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<PagedApiResourceScopeDto> GetPagedAsync(int apiResourceId, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(apiResourceId, page, pageSize);
        }

        public async Task<int> InsertApiResourceScopeAsync(ApiResourceScope apiResourceScope)
        {
            return await _repository.InsertAsync(apiResourceScope);
        }

        public async Task<bool> UpdateApiResourceScopeAsync(ApiResourceScope apiResourceScope)
        {
            return await _repository.UpdateAsync(apiResourceScope);
        }

        public async Task<bool> DeleteApiResourceScopeAsync(ApiResourceScope apiResourceScope)
        {
            return await _repository.DeleteAsync(apiResourceScope);
        }
    }
}
