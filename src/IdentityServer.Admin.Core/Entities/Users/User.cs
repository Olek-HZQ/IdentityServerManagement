using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.Users
{
    [Table(TableNameConstant.User)]
    public class User
    {
        public User()
        {
            UserRoleMaps = new List<UserRoleMapping>();
        }

        public int Id { get; set; }

        public string SubjectId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreationTime { get; set; }

        [Computed]
        public List<UserRoleMapping> UserRoleMaps { get; set; }
    }
}
