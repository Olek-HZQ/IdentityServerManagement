using IdentityServer.Admin.Core.Entities.Localization;

namespace IdentityServer.Admin.Core
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }
    }
}
