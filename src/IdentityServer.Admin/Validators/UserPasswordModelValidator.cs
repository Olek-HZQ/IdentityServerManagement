using FluentValidation;
using IdentityServer.Admin.Models.User;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class UserPasswordModelValidator : BaseValidator<UserPasswordModel>
    {
        public UserPasswordModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage(localizationService.GetResourceAsync("Users.Fields.Password.Required").Result);
            RuleFor(x => x.ConfirmPassword).NotEmpty().MinimumLength(8).WithMessage(localizationService.GetResourceAsync("Users.Fields.ConfirmPassword.Required").Result);
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage(localizationService.GetResourceAsync("Users.Fields.Password.Equal").Result);
        }
    }
}
