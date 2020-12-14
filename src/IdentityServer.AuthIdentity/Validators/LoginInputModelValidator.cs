using FluentValidation;
using IdentityServer.AuthIdentity.Quickstart.Account;

namespace IdentityServer.AuthIdentity.Validators
{
    public class LoginInputModelValidator : BaseValidator<LoginInputModel>
    {
        public LoginInputModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Please input the user name.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please input the password.");
        }
    }
}
