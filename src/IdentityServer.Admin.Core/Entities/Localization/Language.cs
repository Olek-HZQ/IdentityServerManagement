using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.Localization
{
    [Table(TableNameConstant.Language)]
    public class Language
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language culture
        /// </summary>
        public string LanguageCulture { get; set; }

        /// <summary>
        /// Gets or sets the unique SEO code
        /// </summary>
        public string UniqueSeoCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
