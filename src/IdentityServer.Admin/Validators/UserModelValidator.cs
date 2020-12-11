using FluentValidation;
using IdentityServer.Admin.Models.User;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class UserModelValidator : BaseValidator<UserModel>
    {
        public UserModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResourceAsync("Users.Fields.Name.Required").Result);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage(localizationService.GetResourceAsync("Users.Fields.Email.Required").Result);
        }
    }
}
