using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiScope;
using IdentityServer.Admin.Dapper.Repositories.ApiScope;

namespace IdentityServer.Admin.Services.ApiScope
{
    public class ApiScopeService : IApiScopeService
    {
        private readonly IApiScopeRepository _repository;

        public ApiScopeService(IApiScopeRepository repository)
        {
            _repository = repository;
        }

        public Task<List<string>> GetScopesAsync(string scope, int limit = 0)
        {
            return _repository.GetScopesAsync(scope, limit);
        }

        public async Task<PagedApiScopeDto> GetPagedAsync(string search, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(search, page, pageSize);
        }

        public async Task<Core.Entities.ApiScope.ApiScope> GetApiScopeByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<int> InsertApiScopeAsync(Core.Entities.ApiScope.ApiScope apiScope)
        {
            return await _repository.InsertAsync(apiScope);
        }

        public async Task<bool> UpdateApiScopeAsync(Core.Entities.ApiScope.ApiScope apiScope)
        {
            return await _repository.UpdateAsync(apiScope);
        }

        public async Task<bool> DeleteApiScopeAsync(Core.Entities.ApiScope.ApiScope apiScope)
        {
            return await _repository.DeleteAsync(apiScope);
        }
    }
}
