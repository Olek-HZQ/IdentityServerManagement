using FluentValidation;
using IdentityServer.Admin.Models.IdentityResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class IdentityResourceModelValidator : BaseValidator<IdentityResourceModel>
    {
        public IdentityResourceModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResourceAsync("IdentityResources.Name.Required").Result);
        }
    }
}
