using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Entities.Common;

namespace IdentityServer.Admin.Dapper.Repositories.Common
{
    public interface IGenericAttributeRepository : IRepository<GenericAttribute>
    {
        Task<GenericAttribute> GetAttributeAsync(string keyGroup, string key, int entityId);
    }
}
