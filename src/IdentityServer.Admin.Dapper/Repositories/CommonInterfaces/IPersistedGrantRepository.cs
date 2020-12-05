using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IPersistedGrantRepository : IRepository<PersistedGrant>
    {
        Task<PagedPersistedGrantDto> GetPagedAsync(string search, int page, int pageSize);

        Task<PagedPersistedGrantByUserDto> GetPagedByUserAsync(string subjectId, int page, int pageSize);

        Task<PersistedGrant> GetByKeyAsync(string key);

        Task<IEnumerable<PersistedGrant>> GetListBySubjectIdAsync(string subjectId);

        Task<bool> DeleteByKeyAsync(string key);

        Task<bool> DeleteAllBySubjectIdAsync(string subjectId);
    }
}
