using FluentValidation;
using IdentityServer.Admin.Models.User;

namespace IdentityServer.Admin.Validators
{
    public class UserClaimModelValidator : BaseValidator<UserClaimModel>
    {
        public UserClaimModelValidator()
        {
            RuleFor(x => x.ClaimType).NotEmpty().WithMessage("声明类型不能为空");
            RuleFor(x => x.ClaimValue).NotEmpty().WithMessage("声明值不能为空");
        }
    }
}
