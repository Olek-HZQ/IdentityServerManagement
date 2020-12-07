using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.ApiScope;

namespace IdentityServer.Admin.Dapper.Repositories.ApiScope
{
    public interface IApiScopeRepository : IRepository<Core.Entities.ApiScope.ApiScope>
    {
        Task<List<string>> GetScopesAsync(string scope, int limit);

        Task<List<Core.Entities.ApiScope.ApiScope>> GetAllApiScopeListAsync();

        Task<List<Core.Entities.ApiScope.ApiScope>> GetApiScopesByNameAsync(string[] scopeNames);

        Task<PagedApiScopeDto> GetPagedAsync(string search, int page, int pageSize);
    }
}
