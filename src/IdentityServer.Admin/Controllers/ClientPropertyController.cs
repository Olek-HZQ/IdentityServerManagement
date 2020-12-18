using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Client;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Controllers
{
    public class ClientPropertyController : BaseController
    {
        private readonly IClientPropertyService _clientPropertyService;
        private readonly ILocalizationService _localizationService;

        public ClientPropertyController(IClientPropertyService clientPropertyService,ILocalizationService localizationService)
        {
            _clientPropertyService = clientPropertyService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Index(int id, int? page)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Client");
            }

            var pagedClientProperties = await _clientPropertyService.GetPagedAsync(id, page ?? 1);

            ViewBag.pagedClientProperties = pagedClientProperties;

            return View(new ClientPropertyModel
            {
                ClientId = pagedClientProperties.ClientId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientPropertyModel model)
        {
            await _clientPropertyService.InsertClientPropertyAsync(ClientMappers.Mapper.Map<ClientProperty>(model));
            SuccessNotification(await _localizationService.GetResourceAsync("Clients.ClientProperty.Added"));

            return RedirectToAction(nameof(Index), new { Id = model.ClientId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, string name)
        {
            var clientSecret = await _clientPropertyService.GetClientPropertyById(id);

            if (clientSecret == null)
            {
                return RedirectToAction("Index", "Client");
            }

            var model = ClientMappers.Mapper.Map<ClientPropertyModel>(clientSecret);

            ViewBag.ClientName = name;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientPropertyModel model)
        {
            var result = await _clientPropertyService.DeleteClientPropertyAsync(ClientMappers.Mapper.Map<ClientProperty>(model));

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("Clients.ClientProperty.Deleted"));

                return RedirectToAction(nameof(Index), new { Id = model.ClientId });
            }

            return View(model);
        }
    }
}
