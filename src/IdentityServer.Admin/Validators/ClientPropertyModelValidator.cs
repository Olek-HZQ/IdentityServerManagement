using FluentValidation;
using IdentityServer.Admin.Models.Client;

namespace IdentityServer.Admin.Validators
{
    public class ClientPropertyModelValidator : BaseValidator<ClientPropertyModel>
    {
        public ClientPropertyModelValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("键不能为空");
            RuleFor(x => x.Value).NotEmpty().WithMessage("值不能为空");
        }
    }
}
