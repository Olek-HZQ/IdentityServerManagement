using FluentValidation;
using IdentityServer.Admin.Models;

namespace IdentityServer.Admin.Validators
{
    public class UserModelValidator : BaseValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("邮箱格式不对");
        }
    }
}
