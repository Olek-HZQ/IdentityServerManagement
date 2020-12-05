using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer4.Services;

namespace IdentityServer.Admin.Services.Stores
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IClientRepository _clientRepository;

        public CorsPolicyService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var origins = await _clientRepository.GetClientCorsOriginList(origin);

            return origins.Distinct().Contains(origin);
        }
    }
}
