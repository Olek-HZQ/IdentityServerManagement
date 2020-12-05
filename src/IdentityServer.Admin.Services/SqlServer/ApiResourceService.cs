using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
{
    public class ApiResourceService : IApiResourceService
    {
        private readonly IApiResourceRepository _repository;

        public ApiResourceService(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResource> GetApiResourceByIdAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<PagedApiResourceDto> GetPagedAsync(string name, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(name, page, pageSize);
        }

        public async Task<int> InsertApiResourceAsync(ApiResource apiResource)
        {
            return await _repository.InsertAsync(apiResource);
        }

        public async Task<bool> UpdateApiResourceAsync(ApiResource apiResource)
        {
            return await _repository.UpdateAsync(apiResource);
        }

        public async Task<bool> DeleteApiResourceAsync(ApiResource apiResource)
        {
            return await _repository.DeleteAsync(apiResource);
        }
    }
}
