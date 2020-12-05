using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourceScopeModelValidator:BaseValidator<ApiResourceScopeModel>
    {
        public ApiResourceScopeModelValidator()
        {
            RuleFor(x => x.Scope).NotEmpty().WithMessage("名称不能为空");
        }
    }
}
