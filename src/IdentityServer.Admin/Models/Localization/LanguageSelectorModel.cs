using System.Collections.Generic;

namespace IdentityServer.Admin.Models.Localization
{
    public class LanguageSelectorModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }
    }
}
