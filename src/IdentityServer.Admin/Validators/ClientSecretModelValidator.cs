using FluentValidation;
using IdentityServer.Admin.Models.Client;

namespace IdentityServer.Admin.Validators
{
    public class ClientSecretModelValidator:BaseValidator<ClientSecretModel>
    {
        public ClientSecretModelValidator()
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("密钥值不能为空");
        }
    }
}
