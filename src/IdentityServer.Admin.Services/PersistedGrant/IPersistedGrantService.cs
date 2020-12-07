using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.PersistedGrant;

namespace IdentityServer.Admin.Services.PersistedGrant
{
    public interface IPersistedGrantService
    {
        Task<PagedPersistedGrantDto> GetPagedAsync(string search, int page = 1, int pageSize = PageConstant.PageSize);

        Task<PagedPersistedGrantByUserDto> GetPagedByUserAsync(string subjectId, int page, int pageSize = PageConstant.PageSize);

        Task<Core.Entities.PersistedGrant> GetByKeyAsync(string key);

        Task<bool> DeleteByKeyAsync(string key);

        Task<bool> DeleteAllBySubjectIdAsync(string subjectId);
    }
}
