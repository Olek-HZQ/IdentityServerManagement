using FluentValidation;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourceSecretModelValidator : BaseValidator<ApiResourceSecretModel>
    {
        public ApiResourceSecretModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResourceAsync("ApiResourceSecret.Value.Required").Result);
        }
    }
}
