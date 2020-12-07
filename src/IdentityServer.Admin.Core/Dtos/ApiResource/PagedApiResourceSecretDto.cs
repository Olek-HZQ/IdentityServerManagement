using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Core.Dtos.ApiResource
{
    public class PagedApiResourceSecretDto : BasePagedDto<ApiResourceSecretForPage>
    {
        public int ApiResourceId { get; set; }

        public string ApiResourceName { get; set; }
    }

    public class ApiResourceSecretForPage : Secret
    {
        
    }
}
