using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
{
    public class ApiResourceSecretService : IApiResourceSecretService
    {
        private readonly IApiResourceSecretRepository _repository;

        public ApiResourceSecretService(IApiResourceSecretRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResourceSecret> GetApiResourceSecretById(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<PagedApiResourceSecretDto> GetPagedAsync(int apiResourceId, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(apiResourceId, page, pageSize);
        }

        public async Task<int> InsertApiResourceSecret(ApiResourceSecret apiResourceSecret)
        {
            return await _repository.InsertAsync(apiResourceSecret);
        }

        public async Task<bool> UpdateApiResourceSecret(ApiResourceSecret apiResourceSecret)
        {
            return await _repository.UpdateAsync(apiResourceSecret);
        }

        public async Task<bool> DeleteApiResourceSecret(ApiResourceSecret apiResourceSecret)
        {
            return await _repository.DeleteAsync(apiResourceSecret);
        }
    }
}
