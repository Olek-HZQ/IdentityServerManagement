using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace IdentityServer.Admin.Controllers
{
    [Authorize(Policy = AuthorizationConstant.AdministrationPolicy)]
    public class BaseController : Controller
    {
        protected void SuccessNotification(string message, string title = "")
        {
            CreateNotification(NotificationHelper.AlertType.Success, message, title);
        }

        protected void CreateNotification(NotificationHelper.AlertType type, string message, string title = "")
        {
            var toast = new NotificationHelper.Alert
            {
                Type = type,
                Message = message,
                Title = title
            };

            var alerts = new List<NotificationHelper.Alert>();

            if (TempData.ContainsKey(NotificationHelper.NotificationKey))
            {
                alerts = JsonConvert.DeserializeObject<List<NotificationHelper.Alert>>(TempData[NotificationHelper.NotificationKey].ToString());
                TempData.Remove(NotificationHelper.NotificationKey);
            }

            alerts.Add(toast);

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var alertJson = JsonConvert.SerializeObject(alerts, settings);

            TempData.Add(NotificationHelper.NotificationKey, alertJson);
        }

        protected void GenerateNotifications()
        {
            if (!TempData.ContainsKey(NotificationHelper.NotificationKey)) return;
            ViewBag.Notifications = TempData[NotificationHelper.NotificationKey];
            TempData.Remove(NotificationHelper.NotificationKey);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GenerateNotifications();

            base.OnActionExecuting(context);
        }
    }
}
