using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Services.CommonInterfaces
{
    public interface IPersistedGrantService
    {
        Task<PagedPersistedGrantDto> GetPagedAsync(string search, int page = 1, int pageSize = PageConstant.PageSize);

        Task<PagedPersistedGrantByUserDto> GetPagedByUserAsync(string subjectId, int page, int pageSize = PageConstant.PageSize);

        Task<PersistedGrant> GetByKeyAsync(string key);

        Task<bool> DeleteByKeyAsync(string key);

        Task<bool> DeleteAllBySubjectIdAsync(string subjectId);
    }
}
