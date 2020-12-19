using FluentValidation;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourceModelValidator : BaseValidator<ApiResourceModel>
    {
        public ApiResourceModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResourceAsync("ApiResources.Fields.Name.Required").Result);
        }
    }
}
