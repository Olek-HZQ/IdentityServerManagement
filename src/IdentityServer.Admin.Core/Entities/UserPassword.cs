using System;

namespace IdentityServer.Admin.Core.Entities
{
    public class UserPassword
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
