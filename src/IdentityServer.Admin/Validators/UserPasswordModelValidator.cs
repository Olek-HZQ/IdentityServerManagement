using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class UserPasswordModelValidator : BaseValidator<UserPasswordModel>
    {
        public UserPasswordModelValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("密码不能为空");
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(8).WithMessage("确认密码不能为空");
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage("密码跟确认密码不一样");
        }
    }
}
