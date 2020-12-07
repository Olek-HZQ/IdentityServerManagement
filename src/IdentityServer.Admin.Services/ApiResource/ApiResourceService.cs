using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.ApiResource;
using IdentityServer.Admin.Dapper.Repositories.ApiResource;

namespace IdentityServer.Admin.Services.ApiResource
{
    public class ApiResourceService : IApiResourceService
    {
        private readonly IApiResourceRepository _repository;

        public ApiResourceService(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Core.Entities.ApiResource.ApiResource> GetApiResourceByIdAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<PagedApiResourceDto> GetPagedAsync(string name, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(name, page, pageSize);
        }

        public async Task<int> InsertApiResourceAsync(Core.Entities.ApiResource.ApiResource apiResource)
        {
            return await _repository.InsertAsync(apiResource);
        }

        public async Task<bool> UpdateApiResourceAsync(Core.Entities.ApiResource.ApiResource apiResource)
        {
            return await _repository.UpdateAsync(apiResource);
        }

        public async Task<bool> DeleteApiResourceAsync(Core.Entities.ApiResource.ApiResource apiResource)
        {
            return await _repository.DeleteAsync(apiResource);
        }
    }
}
