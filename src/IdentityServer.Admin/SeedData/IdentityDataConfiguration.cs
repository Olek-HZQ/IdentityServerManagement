using System.Collections.Generic;

namespace IdentityServer.Admin.SeedData
{
    public class IdentityDataConfiguration
    {
        public UserSeed User { get; set; }

        public List<RoleSeed> Roles { get; set; }
    }
}
