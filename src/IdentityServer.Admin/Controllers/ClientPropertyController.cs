using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models.Client;
using IdentityServer.Admin.Services.Client;

namespace IdentityServer.Admin.Controllers
{
    public class ClientPropertyController : BaseController
    {
        private readonly IClientPropertyService _clientPropertyService;

        public ClientPropertyController(IClientPropertyService clientPropertyService)
        {
            _clientPropertyService = clientPropertyService;
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
            SuccessNotification("客户端属性添加成功", "成功");

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
                SuccessNotification("客户端属性删除成功", "成功");

                return RedirectToAction(nameof(Index), new { Id = model.ClientId });
            }

            return View(model);
        }
    }
}
