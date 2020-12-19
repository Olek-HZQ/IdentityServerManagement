using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Common;
using IdentityServer.Admin.Dapper.Repositories.Common;
using IdentityServer.Admin.Dapper.Repositories.User;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Admin.Services.Common
{
    public class GenericAttributeService : IGenericAttributeService
    {
        private readonly IGenericAttributeRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public GenericAttributeService(IGenericAttributeRepository repository, IUserRepository userRepository, IHttpContextAccessor httpContext)
        {
            _repository = repository;
            _userRepository = userRepository;
            _httpContext = httpContext;
        }

        private object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }

        private async Task<Core.Entities.Users.User> GetCurrentUserAsync()
        {
            var principal = _httpContext.HttpContext?.User;

            var subjectId = principal?.Claims.FirstOrDefault(x => x.Type == "sub");
            var user = await _userRepository.GetUserBySubjectId(subjectId?.Value);

            return user;
        }

        public async Task<TPropType> GetAttributeAsync<TPropType>(string keyGroup, string key)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var genericAttribute = await _repository.GetAttributeAsync(keyGroup, key, user.Id);
                return genericAttribute != null ? (TPropType)To(genericAttribute.Value, typeof(TPropType), CultureInfo.InvariantCulture) : default;
            }

            return default;
        }

        public async Task<int> InsertAttributeAsync(GenericAttribute genericAttribute)
        {
            return await _repository.InsertAsync(genericAttribute);
        }

        public async Task<bool> UpdateAttributeAsync(GenericAttribute genericAttribute)
        {
            return await _repository.UpdateAsync(genericAttribute);
        }

        public async Task<bool> DeleteAttributeAsync(GenericAttribute genericAttribute)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                genericAttribute.EntityId = user.Id;
                return await _repository.DeleteAsync(genericAttribute);
            }

            return false;
        }

        public async Task SaveAttributeAsync<TPropType>(string keyGroup, string key, TPropType value)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var valueStr = (string)To(value, typeof(string), CultureInfo.InvariantCulture);
                var genericAttribute = await _repository.GetAttributeAsync(keyGroup, key, user.Id);
                if (genericAttribute != null)
                {
                    if (string.IsNullOrEmpty(valueStr))
                    {
                        await DeleteAttributeAsync(genericAttribute);
                    }
                    else
                    {
                        genericAttribute.Value = valueStr;
                        genericAttribute.CreationTime = DateTime.Now;
                        await UpdateAttributeAsync(genericAttribute);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(valueStr))
                    {
                        await InsertAttributeAsync(new GenericAttribute
                        {
                            KeyGroup = keyGroup,
                            Key = key,
                            EntityId = user.Id,
                            Value = valueStr,
                            CreationTime = DateTime.Now
                        });
                    }
                }
            }
        }
    }
}
