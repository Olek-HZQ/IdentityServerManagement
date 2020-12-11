using FluentValidation;
using IdentityServer.Admin.Models.Role;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class RoleModelValidator : BaseValidator<RoleModel>
    {
        public RoleModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResourceAsync("Roles.Field.Name.Required").Result);
            RuleFor(x => x.SystemName).NotEmpty().WithMessage(localizationService.GetResourceAsync("Roles.Fields.SystemName.Required").Result);
        }
    }
}
