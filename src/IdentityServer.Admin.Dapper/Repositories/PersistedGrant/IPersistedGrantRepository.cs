using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos.PersistedGrant;

namespace IdentityServer.Admin.Dapper.Repositories.PersistedGrant
{
    public interface IPersistedGrantRepository : IRepository<Core.Entities.PersistedGrant>
    {
        Task<PagedPersistedGrantDto> GetPagedAsync(string search, int page, int pageSize);

        Task<PagedPersistedGrantByUserDto> GetPagedByUserAsync(string subjectId, int page, int pageSize);

        Task<Core.Entities.PersistedGrant> GetByKeyAsync(string key);

        Task<IEnumerable<Core.Entities.PersistedGrant>> GetListBySubjectIdAsync(string subjectId);

        Task<bool> DeleteByKeyAsync(string key);

        Task<bool> DeleteAllBySubjectIdAsync(string subjectId);
    }
}
