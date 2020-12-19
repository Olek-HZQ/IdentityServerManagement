// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System.Collections.Generic;

namespace IdentityServer.Admin.Models.ApiScope
{
    public class ApiScopeModel
    {
        public ApiScopeModel()
        {
            UserClaims = new List<ApiScopeClaimModel>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiScopeClaimModel> UserClaims { get; set; }
        public List<ApiScopePropertyModel> Properties { get; set; }

        public string UserClaimsItems { get; set; }
    }
}