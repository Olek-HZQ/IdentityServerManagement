using System.Linq;
using IdentityServer.Admin.Core;
using IdentityServer.Admin.Models.Localization;
using IdentityServer.Admin.Services.Localization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Admin.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;

        public LanguageSelectorViewComponent(ILanguageService languageService, IWorkContext workContext)
        {
            _languageService = languageService;
            _workContext = workContext;
        }

        public IViewComponentResult Invoke()
        {
            var availableLanguages =  _languageService.GetAllLanguagesAsync().Result
                .Select(x => new LanguageModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            var model = new LanguageSelectorModel
            {
                CurrentLanguageId = _workContext.WorkingLanguage.Id,
                AvailableLanguages = availableLanguages
            };

            if (model.AvailableLanguages.Count == 1)
                return Content("");

            return View(model);
        }
    }
}
