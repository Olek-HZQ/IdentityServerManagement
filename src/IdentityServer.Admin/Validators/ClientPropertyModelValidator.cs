using FluentValidation;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ClientPropertyModelValidator : BaseValidator<ClientPropertyModel>
    {
        public ClientPropertyModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage(localizationService.GetResourceAsync("Clients.ClientProperty.Key.Required").Result);
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResourceAsync("Clients.ClientProperty.Value.Required").Result);
        }
    }
}
