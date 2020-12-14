using FluentValidation;
using IdentityServer.Admin.Models.User;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class UserClaimModelValidator : BaseValidator<UserClaimModel>
    {
        public UserClaimModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ClaimType).NotEmpty().WithMessage(localizationService.GetResourceAsync("Users.UserClaims.Type.Required").Result);
            RuleFor(x => x.ClaimValue).NotEmpty().WithMessage(localizationService.GetResourceAsync("Users.UserClaims.Value.Required").Result);
        }
    }
}
