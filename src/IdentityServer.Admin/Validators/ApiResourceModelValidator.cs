using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourceModelValidator:BaseValidator<ApiResourceModel>
    {
        public ApiResourceModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
        }
    }
}
