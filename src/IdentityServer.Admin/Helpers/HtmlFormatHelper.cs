using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
