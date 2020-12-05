using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ApiScopeModelValidator : BaseValidator<ApiScopeModel>
    {
        public ApiScopeModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
        }
    }
}
