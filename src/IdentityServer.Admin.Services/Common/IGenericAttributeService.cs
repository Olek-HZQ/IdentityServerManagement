using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Common;

namespace IdentityServer.Admin.Services.Common
{
    public interface IGenericAttributeService
    {
        Task<TPropType> GetAttributeAsync<TPropType>(string keyGroup, string key);

        Task<int> InsertAttributeAsync(GenericAttribute genericAttribute);

        Task<bool> UpdateAttributeAsync(GenericAttribute genericAttribute);

        Task<bool> DeleteAttributeAsync(GenericAttribute genericAttribute);

        Task SaveAttributeAsync<TPropType>(string keyGroup, string key, TPropType value);
    }
}
