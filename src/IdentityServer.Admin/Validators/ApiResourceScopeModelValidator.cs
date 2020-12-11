using FluentValidation;
using IdentityServer.Admin.Models.ApiResource;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ApiResourceScopeModelValidator : BaseValidator<ApiResourceScopeModel>
    {
        public ApiResourceScopeModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Scope).NotEmpty().WithMessage(localizationService.GetResourceAsync("ApiResourceScope.Fields.Name.Required").Result);
        }
    }
}
