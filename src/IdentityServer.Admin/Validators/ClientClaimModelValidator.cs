using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ClientClaimModelValidator:BaseValidator<ClientClaimModel>
    {
        public ClientClaimModelValidator()
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("声明值不能为空");
        }
    }
}
