using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Services.SqlServer
{
    public class ClientSecretService : IClientSecretService
    {
        private readonly IClientSecretRepository _repository;

        public ClientSecretService(IClientSecretRepository repository)
        {
            _repository = repository;
        }

        public Task<PagedClientSecretDto> GetPagedAsync(int clientId, int page = 1, int pageSize = PageConstant.PageSize)
        {
            return _repository.GetPagedAsync(clientId, page, pageSize);
        }

        public async Task<ClientSecret> GetClientSecretById(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<int> InsertClientSecretAsync(ClientSecret clientSecret)
        {
            return await _repository.InsertAsync(clientSecret);
        }

        public async Task<bool> DeleteClientSecretAsync(ClientSecret clientSecret)
        {
            return await _repository.DeleteAsync(clientSecret);
        }
    }
}
