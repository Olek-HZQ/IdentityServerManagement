using FluentValidation;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Validators
{
    public class ClientClaimModelValidator:BaseValidator<ClientClaimModel>
    {
        public ClientClaimModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResourceAsync("Clients.ClientClaims.Value.Required").Result);
        }
    }
}
