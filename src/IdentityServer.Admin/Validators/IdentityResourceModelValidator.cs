using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class IdentityResourceModelValidator : BaseValidator<IdentityResourceModel>
    {
        public IdentityResourceModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
        }
    }
}
