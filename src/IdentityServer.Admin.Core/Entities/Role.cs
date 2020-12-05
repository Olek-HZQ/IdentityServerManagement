using Dapper.Contrib.Extensions;

namespace IdentityServer.Admin.Core.Entities
{
    [Table("Roles")]
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SystemName { get; set; }
    }
}
