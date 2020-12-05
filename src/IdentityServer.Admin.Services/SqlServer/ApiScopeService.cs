using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
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

        public async Task<ApiScope> GetApiScopeByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<int> InsertApiScopeAsync(ApiScope apiScope)
        {
            return await _repository.InsertAsync(apiScope);
        }

        public async Task<bool> UpdateApiScopeAsync(ApiScope apiScope)
        {
            return await _repository.UpdateAsync(apiScope);
        }

        public async Task<bool> DeleteApiScopeAsync(ApiScope apiScope)
        {
            return await _repository.DeleteAsync(apiScope);
        }
    }
}
