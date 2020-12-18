using System.Collections.Generic;
using IdentityServer.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace IdentityServer.Admin.ExceptionHandling
{
    public class ControllerExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public ControllerExceptionFilterAttribute(ITempDataDictionaryFactory tempDataDictionaryFactory,
            IModelMetadataProvider modelMetadataProvider)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            //Create toastr notification
            if (CreateNotification(context, out var tempData)) return;

            ProcessException(context, tempData);

            //Clear toastr notification from temp
            ClearNotification(tempData);
        }

        private static void ClearNotification(ITempDataDictionary tempData)
        {
            tempData.Remove(NotificationHelper.NotificationKey);
        }

        private bool CreateNotification(ExceptionContext context, out ITempDataDictionary tempData)
        {
            tempData = _tempDataDictionaryFactory.GetTempData(context.HttpContext);
            CreateNotification(NotificationHelper.AlertType.Error, tempData, context.Exception.Message);

            return !tempData.ContainsKey(NotificationHelper.NotificationKey);
        }

        private void ProcessException(ExceptionContext context, ITempDataDictionary tempData)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor)) return;

            const string errorViewName = "Error";

            var result = new ViewResult
            {
                ViewName = errorViewName,
                TempData = tempData,
                ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
                {
                    {"Notifications", tempData[NotificationHelper.NotificationKey]},
                }
            };

            context.ExceptionHandled = true;
            context.Result = result;
        }

        protected void CreateNotification(NotificationHelper.AlertType type, ITempDataDictionary tempData, string message, string title = "")
        {
            var toast = new NotificationHelper.Alert
            {
                Type = type,
                Message = message,
                Title = title
            };

            var alerts = new List<NotificationHelper.Alert>();

            if (tempData.ContainsKey(NotificationHelper.NotificationKey))
            {
                alerts = JsonConvert.DeserializeObject<List<NotificationHelper.Alert>>(tempData[NotificationHelper.NotificationKey].ToString());
                tempData.Remove(NotificationHelper.NotificationKey);
            }

            alerts.Add(toast);

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var alertJson = JsonConvert.SerializeObject(alerts, settings);

            tempData.Add(NotificationHelper.NotificationKey, alertJson);
        }
    }
}





