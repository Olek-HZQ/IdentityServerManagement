using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.PersistedGrant;
using IdentityServer.Admin.Dapper.Repositories.PersistedGrant;

namespace IdentityServer.Admin.Services.PersistedGrant
{
    public class PersistedGrantService : IPersistedGrantService
    {
        private readonly IPersistedGrantRepository _repository;

        public PersistedGrantService(IPersistedGrantRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedPersistedGrantDto> GetPagedAsync(string search, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(search, page, pageSize);
        }

        public async Task<PagedPersistedGrantByUserDto> GetPagedByUserAsync(string subjectId, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedByUserAsync(subjectId, page, pageSize);
        }

        public async Task<Core.Entities.PersistedGrant> GetByKeyAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return await _repository.GetByKeyAsync(key);
        }

        public async Task<bool> DeleteByKeyAsync(string key)
        {
            return await _repository.DeleteByKeyAsync(key);
        }

        public async Task<bool> DeleteAllBySubjectIdAsync(string subjectId)
        {
            return await _repository.DeleteAllBySubjectIdAsync(subjectId);
        }
    }
}
