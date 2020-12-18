using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Client;
using IdentityServer.Admin.Services.Localization;

namespace IdentityServer.Admin.Controllers
{
    public class ClientClaimController : BaseController
    {
        private readonly IClientClaimService _clientClaimService;
        private readonly ILocalizationService _localizationService;

        public ClientClaimController(IClientClaimService clientClaimService,ILocalizationService localizationService)
        {
            _clientClaimService = clientClaimService;
            _localizationService = localizationService;
        }

        public async Task<IActionResult> Index(int id, int? page)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Client");
            }

            var pagedClientClaims = await _clientClaimService.GetPagedAsync(id, page ?? 1);

            ViewBag.PagedClientClaims = pagedClientClaims;

            return View(new ClientClaimModel
            {
                ClientId = pagedClientClaims.ClientId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientClaimModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), new { id = model.Id });
            }

            await _clientClaimService.InsertClientClaim(ClientMappers.Mapper.Map<ClientClaim>(model));
            SuccessNotification(await _localizationService.GetResourceAsync("Clients.ClientClaims.Added"));

            return RedirectToAction(nameof(Index), new { Id = model.ClientId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var clientSecret = await _clientClaimService.GetClientClaimById(id);

            if (clientSecret == null)
            {
                return RedirectToAction("Index", "Client");
            }

            var model = ClientMappers.Mapper.Map<ClientClaimModel>(clientSecret);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientClaimModel model)
        {
            var result = await _clientClaimService.DeleteClientClaim(ClientMappers.Mapper.Map<ClientClaim>(model));

            if (result)
            {
                SuccessNotification(await _localizationService.GetResourceAsync("Clients.ClientClaims.Deleted"));
            }

            return RedirectToAction(nameof(Index), new { Id = model.ClientId });
        }

        [HttpGet]
        public IActionResult SearchClaims(string claim, int limit = 0)
        {
            var claims = ClientConstant.GetStandardClaims();

            if (!string.IsNullOrEmpty(claim))
            {
                claims = claims.Where(x => x.Contains(claim)).ToList();
            }

            if (limit > 0)
            {
                claims = claims.Take(limit).ToList();
            }

            return Ok(claims);
        }
    }
}
