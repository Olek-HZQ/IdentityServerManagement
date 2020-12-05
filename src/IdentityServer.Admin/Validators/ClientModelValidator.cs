using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ClientModelValidator : BaseValidator<ClientModel>
    {
        public ClientModelValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("客户端标识不能为空");
            RuleFor(x => x.ClientName).NotEmpty().WithMessage("客户端名称不能为空");
        }
    }
}
