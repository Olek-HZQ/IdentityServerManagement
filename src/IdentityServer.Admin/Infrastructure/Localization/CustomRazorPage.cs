using IdentityServer.Admin.Services.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Admin.Infrastructure.Localization
{
    public abstract class CustomRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private Localizer _localizer;

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer T
        {
            get
            {
                var localizationService = (LocalizationService)Context.RequestServices.GetRequiredService(typeof(ILocalizationService));
                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                    {
                        var resFormat = localizationService.GetResourceAsync(format).Result;
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return new LocalizedString(args == null || args.Length == 0
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }
    }
}
