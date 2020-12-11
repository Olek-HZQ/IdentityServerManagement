using FluentValidation;
using IdentityServer.Admin.Models.ApiScope;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ApiScopeModelValidator : BaseValidator<ApiScopeModel>
    {
        public ApiScopeModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResourceAsync("ApiScope.Fields.Name.Required").Result);
        }
    }
}
