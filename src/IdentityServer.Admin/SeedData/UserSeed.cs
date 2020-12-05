using System.Collections.Generic;

namespace IdentityServer.Admin.SeedData
{
    public class UserSeed
    {
        public UserSeed()
        {
            Claims = new List<ClaimSeed>();
            Roles = new List<string>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<ClaimSeed> Claims { get; set; } = new List<ClaimSeed>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}
