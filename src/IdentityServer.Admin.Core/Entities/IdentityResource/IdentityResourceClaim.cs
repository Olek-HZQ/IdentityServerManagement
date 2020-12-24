// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

#pragma warning disable 1591

namespace IdentityServer.Admin.Core.Entities.IdentityResource
{
    [Table(TableNameConstant.IdentityResourceClaim)]
    public class IdentityResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public int IdentityResourceId { get; set; }

        [Computed]
        public virtual IdentityResource IdentityResource { get; set; }
    }
}