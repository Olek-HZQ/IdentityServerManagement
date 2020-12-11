using System.Globalization;
using System.Threading.Tasks;
using IdentityServer.Admin.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace IdentityServer.Admin.Services.Localization
{
    /// <summary>
    /// Represents middleware that set current culture based on request
    /// </summary>
    public class CultureMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next">Next</param>
        /// <param name="httpContextAccessor"></param>
        public CultureMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Set working culture
        /// </summary>
        /// <param name="workContext">Work context</param>
        protected void SetWorkingCulture(IWorkContext workContext)
        {
            if (_httpContextAccessor?.HttpContext == null)
                return;

            string path = _httpContextAccessor.HttpContext.Request.Path;

            //a little workaround. FileExtensionContentTypeProvider contains most of static file extensions. So we can use it
            //source: https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
            //if it can return content type, then it's a static file
            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (contentTypeProvider.TryGetContentType(path, out var _))
                return;

            //set working language culture
            var culture = new CultureInfo(workContext.WorkingLanguage.LanguageCulture);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke middleware actions
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="workContext">Work context</param>
        /// <returns>Task</returns>
        public Task Invoke(HttpContext context, IWorkContext workContext)
        {
            //set culture
            SetWorkingCulture(workContext);

            //call the next middleware in the request pipeline
            return _next(context);
        }

        #endregion
    }
}
