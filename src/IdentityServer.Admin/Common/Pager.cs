using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Common
{
    public class Pager
    {
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public string Action { get; set; }

        public string Search { get; set; }

        public bool EnableSearch { get; set; }

        public int MaxPages { get; set; } = PageConstant.MaxPages;
    }
}
