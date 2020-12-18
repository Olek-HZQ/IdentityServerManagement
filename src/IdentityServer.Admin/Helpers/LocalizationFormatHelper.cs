using System.Threading.Tasks;
using IdentityServer.Admin.Services.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Admin.Helpers
{
    public static class LocalizationFormatHelper
    {
        public static async Task<string> GetResource(this HttpContext context, string resourceName)
        {
            var localizationService = (LocalizationService)context.RequestServices.GetRequiredService(typeof(ILocalizationService));
            var result = await localizationService.GetResourceAsync(resourceName, defaultValue: "Unknown");
            return result;
        }
    }
}
