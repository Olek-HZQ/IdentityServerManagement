// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

#pragma warning disable 1591

namespace IdentityServer.Admin.Core.Entities.Users
{
    [Table(TableNameConstant.UserClaim)]
    public class UserClaim
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        [Computed]
        public virtual User User { get; set; }
    }
}