using System;
using Dapper.Contrib.Extensions;

namespace IdentityServer.Admin.Core.Entities
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }

        public string SubjectId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
