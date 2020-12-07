using System.Threading.Tasks;
using IdentityServer.Admin.Dapper.Repositories.User;
using IdentityServer.Admin.Services.Security;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace IdentityServer.Admin.Services.Stores
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.GetUserByNameAsync(context.UserName);

            if (user != null)
            {
                var userPassword = await _userRepository.GetPasswordByUserIdAsync(user.Id);

                if (userPassword != null)
                {
                    string encryptPassword = _encryptionService.CreatePasswordHash(context.Password, userPassword.PasswordSalt);

                    if (encryptPassword == userPassword.Password)
                    {
                        context.Result = new GrantValidationResult(subject: user.SubjectId, authenticationMethod: "custom");
                        return;
                    }
                }
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
        }
    }
}
