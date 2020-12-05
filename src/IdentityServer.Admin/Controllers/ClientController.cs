using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities;
using IdentityServer.Admin.Core.Entities.Enums;
using IdentityServer.Admin.Core.Extensions;
using IdentityServer.Admin.Infrastructure.Mappers;
using IdentityServer.Admin.Models;
using IdentityServer.Admin.Services.CommonInterfaces;

namespace IdentityServer.Admin.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly IIdentityResourceService _identityResourceService;
        private readonly IApiScopeService _apiScopeService;

        public ClientController(IClientService clientService, IIdentityResourceService identityResourceService, IApiScopeService apiScopeService)
        {
            _clientService = clientService;
            _identityResourceService = identityResourceService;
            _apiScopeService = apiScopeService;
        }

        #region Client

        public IActionResult Index(string search, int? page)
        {
            var client = _clientService.GetPagedClients(search, page ?? 1);

            ViewBag.Search = search;

            return View(client);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ClientModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = model.ToEntity();

            var allowedGrantTypes = GrantTypes.Code.Select(x => new ClientGrantType
            {
                GrantType = x
            }).ToList();

            switch (model.ClientType)
            {
                case ClientType.Empty:
                    break;
                case ClientType.Web:
                    client.AllowedGrantTypes = allowedGrantTypes;
                    client.RequirePkce = true;
                    client.RequireClientSecret = true;
                    break;
                case ClientType.Spa:
                    client.AllowedGrantTypes = allowedGrantTypes;
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Native:
                    client.AllowedGrantTypes = allowedGrantTypes;
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Machine:
                    client.AllowedGrantTypes = GrantTypes.ClientCredentials.Select(x => new ClientGrantType
                    {
                        GrantType = x
                    }).ToList();
                    break;
                case ClientType.Device:
                    client.AllowedGrantTypes = GrantTypes.DeviceFlow.Select(x => new ClientGrantType
                    {
                        GrantType = x
                    }).ToList();
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var clientId = await _clientService.InsertClientAsync(client);

            if (clientId > 0)
            {
                SuccessNotification($"客户端【{model.ClientName}】添加成功", "成功");

                return RedirectToAction(nameof(Edit), new { id = clientId });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return View(new ClientModel());
            }

            var client = (await _clientService.GetClientByIdAsync(id)).ToModel();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = await _clientService.GetClientByIdAsync(model.Id);

            if (client == null)
            {
                return Index("", 1);
            }

            model.AllowedScopes = model.AllowedScopesItems.Deserialize<List<string>>()?.Select(x => new ClientScopeModel
            {
                Scope = x
            }).ToList();
            model.RedirectUris = model.RedirectUrisItems.Deserialize<List<string>>()?.Select(x => new ClientRedirectUriModel
            {
                RedirectUri = x
            }).ToList();
            model.AllowedGrantTypes = model.AllowedGrantTypesItems.Deserialize<List<string>>()?.Select(x => new ClientGrantTypeModel
            {
                GrantType = x
            }).ToList();
            model.PostLogoutRedirectUris = model.PostLogoutRedirectUrisItems.Deserialize<List<string>>()?.Select(x => new ClientPostLogoutRedirectUriModel
            {
                PostLogoutRedirectUri = x
            }).ToList();
            model.IdentityProviderRestrictions = model.IdentityProviderRestrictionsItems.Deserialize<List<string>>()?.Select(x => new ClientIdPRestrictionModel
            {
                Provider = x
            }).ToList();
            model.AllowedCorsOrigins = model.AllowedCorsOriginsItems.Deserialize<List<string>>()?.Select(x => new ClientCorsOriginModel
            {
                Origin = x
            }).ToList();

            client = model.ToEntity();

            var updatedResult = await _clientService.UpdateClientAsync(client);
            if (updatedResult)
            {
                SuccessNotification($"客户端【{model.ClientName}】编辑成功", "成功");
                return RedirectToAction(nameof(Edit), new { id = client.Id });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            if (client == null)
            {
                return RedirectToAction("Index", "Client");
            }

            var model = ClientMappers.Mapper.Map<ClientModel>(client);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClientModel model)
        {
            var result = await _clientService.DeleteClientAsync(ClientMappers.Mapper.Map<Client>(model));

            if (result)
            {
                SuccessNotification($"客户端【{model.ClientName}】删除成功", "成功");
            }

            return RedirectToAction(nameof(Index), new { model.Id });
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> SearchScopes(string scope, int limit = 0)
        {
            var apiScope = await _apiScopeService.GetScopesAsync(scope, limit);

            var identityResource = await _identityResourceService.GetResourcesAsync(scope, limit);

            return Ok(apiScope.Union(identityResource).Distinct());
        }

        [HttpGet]
        public IActionResult SearchGrantTypes(string grant, int limit = 0)
        {
            var grants = ClientConstant.GetGrantTypes();

            if (!string.IsNullOrEmpty(grant))
            {
                grants = grants.Where(x => x.Contains(grant)).ToList();
            }

            if (limit > 0)
            {
                grants = grants.Take(limit).ToList();
            }

            return Ok(grants);
        }
    }
}
