using FluentValidation;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourcePropertyModelValidator:BaseValidator<ApiResourcePropertyModel>
    {
        public ApiResourcePropertyModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Key).NotEmpty().WithMessage(localizationService.GetResourceAsync("ApiResourceProperty.Key.Required").Result);
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResourceAsync("ApiResourceProperty.Value.Required").Result);
        }
    }
}
