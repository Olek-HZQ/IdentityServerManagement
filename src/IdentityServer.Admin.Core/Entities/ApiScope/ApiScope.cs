// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.ApiScope
{
    [Table(TableNameConstant.ApiScope)]
    public class ApiScope
    {
        public ApiScope()
        {
            UserClaims = new List<ApiScopeClaim>();
            Properties = new List<ApiScopeProperty>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        [Computed]
        public List<ApiScopeClaim> UserClaims { get; set; }
        [Computed]
        public List<ApiScopeProperty> Properties { get; set; }
    }
}