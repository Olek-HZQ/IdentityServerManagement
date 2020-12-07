using System;

namespace IdentityServer.Admin.Core.Dtos.PersistedGrant
{
    public class PagedPersistedGrantDto : BasePagedDto<PersistedGrantForPage>
    {
    }

    public class PersistedGrantForPage
    {
        public string SubjectId { get; set; }

        public string SubjectName { get; set; }
    }

    public class PagedPersistedGrantByUserDto : BasePagedDto<PersistedGrantByUserForPage>
    {

    }

    public class PersistedGrantByUserForPage
    {
        public string Key { get; set; }

        public string Type { get; set; }

        public string SubjectId { get; set; }

        public string SubjectName { get; set; }

        public string ClientId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? Expiration { get; set; }

        public string Data { get; set; }
    }
}
