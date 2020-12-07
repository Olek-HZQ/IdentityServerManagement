using System;

namespace IdentityServer.Admin.Helpers
{
    public class HtmlFormatHelper
    {
        public static Tuple<string, string> FormatLabel(string name, string title)
        {
            return new Tuple<string, string>(name, title);
        }
    }
}
