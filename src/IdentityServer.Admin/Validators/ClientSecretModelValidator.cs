using FluentValidation;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ClientSecretModelValidator:BaseValidator<ClientSecretModel>
    {
        public ClientSecretModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResourceAsync("Clients.ClientSecret.ValureRequired").Result);
        }
    }
}
