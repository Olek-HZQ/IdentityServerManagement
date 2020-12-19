using System;
using System.Linq;
using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Entities.Common;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Core.Entities.Users;
using IdentityServer.Admin.Services.Common;
using IdentityServer.Admin.Services.Localization;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Admin
{
    public class WebWorkContext : IWorkContext
    {
        private readonly ILanguageService _languageService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private Language _cachedLanguage;

        public WebWorkContext(ILanguageService languageService, IGenericAttributeService genericAttributeService, IHttpContextAccessor httpContextAccessor)
        {
            _languageService = languageService;
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Utilities

        /// <summary>
        /// Get language cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetLanguageCookie()
        {
            var cookieName = ".SelectedLanguage";
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }

        /// <summary>
        /// Set language cookie
        /// </summary>
        /// <param name="languageId">language id</param>
        protected virtual void SetLanguageCookie(int languageId)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            var cookieName = ".SelectedLanguage";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpiresDate = DateTime.Now.AddDays(7);

            //if passed guid is empty set cookie as expired
            if (languageId <= 0)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate,
                Secure = _httpContextAccessor.HttpContext.Request.IsHttps
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, languageId.ToString(), options);
        }

        #endregion

        public Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                int languageId;

                if (_httpContextAccessor.HttpContext.User.IsAuthenticated())
                {
                    languageId = _genericAttributeService
                        .GetAttributeAsync<int>(KeyGroupDefaults.UserKeyGroup, UserDefaults.LanguageIdAttribute).Result;
                }
                else
                {
                    int.TryParse(GetLanguageCookie(), out languageId);
                }

                var language = _languageService.GetLanguageByIdAsync(languageId).Result;

                _cachedLanguage = language ?? _languageService.GetAllLanguagesAsync().Result.FirstOrDefault(); // 如果有切换过语言则取配置，否则取默认

                return _cachedLanguage;
            }
            set
            {
                SetLanguageCookie(value.Id);
                _cachedLanguage = value;
            }
        }
    }
}
