using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Core.Dtos
{
    public class PagedApiResourcePropertyDto : BasePagedDto<ApiResourcePropertyForPage>
    {
        public int ApiResourceId { get; set; }

        public string ApiResourceName { get; set; }
    }

    public class ApiResourcePropertyForPage : Property
    {
    }
}
