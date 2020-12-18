// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace IdentityServer.Admin.Models.ApiResource
{
    public class ApiResourceModel
    {
        public ApiResourceModel()
        {
            Secrets = new List<ApiResourceSecretModel>();
            Scopes = new List<ApiResourceScopeModel>();
            UserClaims = new List<ApiResourceClaimModel>();
            Properties = new List<ApiResourcePropertyModel>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiResourceSecretModel> Secrets { get; set; }
        public List<ApiResourceScopeModel> Scopes { get; set; }
        public List<ApiResourceClaimModel> UserClaims { get; set; }
        public List<ApiResourcePropertyModel> Properties { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }

        public string UserClaimsItems { get; set; }
    }
}
