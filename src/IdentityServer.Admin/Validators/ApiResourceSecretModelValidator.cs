using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourceSecretModelValidator : BaseValidator<ApiResourceSecretModel>
    {
        public ApiResourceSecretModelValidator()
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("密钥值不能为空");
        }
    }
}
