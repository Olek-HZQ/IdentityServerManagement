using Dapper.Contrib.Extensions;

namespace IdentityServer.Admin.Core.Entities.Localization
{
    [Table("LocaleStringResource")]
    public class LocaleStringResource
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public string ResourceValue { get; set; }
    }
}
