using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Entities.Common;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Helpers;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.Localization;
using IdentityServer.Admin.Services.Common;
using Microsoft.AspNetCore.Mvc;
using IdentityServer.Admin.Services.Localization;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;

        public LanguageController(ILanguageService languageService, ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService, IWorkContext workContext)
        {
            _languageService = languageService;
            _localizationService = localizationService;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
        }

        #region Language

        public async Task<IActionResult> Index(string search, int? page)
        {
            var pagedLanguages = await _languageService.GetPagedAsync(search, page ?? 1);

            ViewBag.Search = search;

            return View(pagedLanguages);
        }

        public IActionResult Create()
        {
            var model = new LanguageModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LanguageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _languageService.InsertLanguageAsync(model.ToEntity());
            SuccessNotification(await _localizationService.GetResourceAsync("Language.Added"));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var language = await _languageService.GetLanguageByIdAsync(id);

            if (language == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(language.ToModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LanguageModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var language = await _languageService.GetLanguageByIdAsync(model.Id);

            if (language == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var updatedResult = await _languageService.UpdateLanguageAsync(model.ToEntity());

            if (updatedResult)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("Language.Updated"));
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);

            if (language == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(language.ToModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(LanguageModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _languageService.DeleteLanguageAsync(model.ToEntity());

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("Language.Deleted"));
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ChangeLanguage(int id, string returnUrl)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);
            if (language == null || !language.Published)
            {
                language = _workContext.WorkingLanguage;
            }

            if (language != null && HttpContext.User.IsAuthenticated())
            {
                await _genericAttributeService.SaveAttributeAsync(KeyGroupDefaults.UserKeyGroup, UserDefaults.LanguageIdAttribute, language.Id.ToString());
            }

            _workContext.WorkingLanguage = language;

            return LocalRedirect(returnUrl);
        }

        #endregion

        #region LocaleStringResource

        public async Task<IActionResult> ResourceDownload(int languageId)
        {
            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null)
                return new EmptyResult();

            var resources = await _localizationService.GetAllResourceValuesAsync(languageId);
            if (resources.Any())
            {
                Response.Headers.Add($"Content-Disposition", $"attachment; filename=language-{language.Name}.json");
                var downloadData = resources.ToDictionary(x => x.Key, y => y.Value.Value);
                return new FileContentResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(downloadData)), "text/json");
            }

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResourceUpload(int id, IFormFile jsonFile)
        {
            //try to get a language with the specified id
            var language = await _languageService.GetLanguageByIdAsync(id);
            if (language == null)
                return RedirectToAction(nameof(Index));

            try
            {
                if (jsonFile != null && jsonFile.Length > 0)
                {
                    using var sr = new StreamReader(jsonFile.OpenReadStream(), Encoding.UTF8);

                    await _localizationService.SaveResourcesAsync(language.Id, await sr.ReadToEndAsync());
                }
                else
                {
                    CreateNotification(NotificationHelper.AlertType.Info, await _localizationService.GetResourceAsync("Languages.Upload.Empty"));
                    return RedirectToAction(nameof(Edit), new { id = language.Id });
                }

                CreateNotification(NotificationHelper.AlertType.Success, await _localizationService.GetResourceAsync("Languages.Upload.Success"));
                return RedirectToAction(nameof(Edit), new { id = language.Id });
            }
            catch (Exception ex)
            {
                CreateNotification(NotificationHelper.AlertType.Error, await _localizationService.GetResourceAsync(string.Format("Languages.Upload.Filed", ex.Message)));
                return RedirectToAction(nameof(Edit), new { id = language.Id });
            }
        }

        public async Task<IActionResult> Resources(int languageId, string search, int? page)
        {
            var pagedResources = await _localizationService.GetPagedAsync(languageId, search, page ?? 1);

            ViewBag.Search = search;

            return View(pagedResources);
        }

        public IActionResult ResourceCreate(int languageId)
        {
            var model = new LocaleStringResourceModel
            {
                LanguageId = languageId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResourceCreate(LocaleStringResourceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _localizationService.InsertStringResourceAsync(model.ToLocaleStringResource());
            SuccessNotification(await _localizationService.GetResourceAsync("LocaleStringResource.Added"));

            return RedirectToAction(nameof(Resources), new { languageId = model.LanguageId });
        }

        [HttpGet]
        public async Task<IActionResult> ResourceEdit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Resources));
            }

            var resource = await _localizationService.GetStringResourceByIdAsync(id);

            if (resource == null)
            {
                return RedirectToAction(nameof(Resources));
            }

            return View(resource.ToLocaleStringResourceModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResourceEdit(LocaleStringResourceModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Resources));
            }

            var resource = await _localizationService.GetStringResourceByIdAsync(model.Id);

            if (resource == null)
            {
                return RedirectToAction(nameof(Resources), new { languageId = model.LanguageId });
            }

            var updatedResult = await _localizationService.UpdateStringResourceAsync(model.ToLocaleStringResource());

            if (updatedResult)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("LocaleStringResource.Updated"));
                return RedirectToAction(nameof(Resources), new { languageId = model.LanguageId });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResourceDelete(int id)
        {
            var resource = await _localizationService.GetStringResourceByIdAsync(id);

            if (resource == null)
            {
                return RedirectToAction(nameof(Resources));
            }

            return View(resource.ToLocaleStringResourceModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResourceDelete(LocaleStringResourceModel model)
        {
            if (model.Id == 0)
            {
                return RedirectToAction(nameof(Resources));
            }

            var result = await _localizationService.DeleteStringResourceAsync(model.ToLocaleStringResource());

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("LocaleStringResource.Deleted"));
                return RedirectToAction(nameof(Resources), new { languageId = model.LanguageId });
            }

            return RedirectToAction(nameof(Resources));
        }

        #endregion
    }
}
