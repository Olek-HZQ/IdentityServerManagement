namespace IdentityServer.Admin.Core.Dtos.Localization
{
    public class PagedLocalStringResourceDto : BasePagedDto<LocalStringResourceForPage>
    {
        public int LanguageId { get; set; }

        public string LanguageName { get; set; }
    }

    public class LocalStringResourceForPage
    {
        public int Id { get; set; }

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
