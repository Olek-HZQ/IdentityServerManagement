using System;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.Users
{
    [Table(TableNameConstant.UserPassword)]
    public class UserPassword
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime CreationTime { get; set; }

        [Computed]
        public virtual User User { get; set; }
    }
}
