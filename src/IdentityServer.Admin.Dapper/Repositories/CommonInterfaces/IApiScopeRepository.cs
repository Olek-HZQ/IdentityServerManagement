using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IApiScopeRepository : IRepository<ApiScope>
    {
        Task<List<string>> GetScopesAsync(string scope, int limit);

        Task<List<ApiScope>> GetAllApiScopeListAsync();

        Task<List<ApiScope>> GetApiScopesByNameAsync(string[] scopeNames);

        Task<PagedApiScopeDto> GetPagedAsync(string search, int page, int pageSize);
    }
}
