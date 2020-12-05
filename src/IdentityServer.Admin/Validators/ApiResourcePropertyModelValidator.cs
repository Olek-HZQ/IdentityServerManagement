using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourcePropertyModelValidator:BaseValidator<ApiResourcePropertyModel>
    {
        public ApiResourcePropertyModelValidator()
        {
            RuleFor(x => x.Key).NotEmpty().WithMessage("键不能为空");
            RuleFor(x => x.Value).NotEmpty().WithMessage("值不能为空");
        }
    }
}
