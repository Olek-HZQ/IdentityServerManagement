using FluentValidation;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ClientModelValidator : BaseValidator<ClientModel>
    {
        public ClientModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage(localizationService.GetResourceAsync("Clients.ClientId.Required").Result);
            RuleFor(x => x.ClientName).NotEmpty().WithMessage(localizationService.GetResourceAsync("Clients.ClientName.Required").Result);
        }
    }
}
