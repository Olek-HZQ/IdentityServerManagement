// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace IdentityServer.Admin.Core.Entities
{
    [Table("ApiResources")]
    public class ApiResource
    {
        public ApiResource()
        {
            Secrets = new List<ApiResourceSecret>();
            Scopes = new List<ApiResourceScope>();
            UserClaims = new List<ApiResourceClaim>();
            Properties = new List<ApiResourceProperty>();
        }

        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        [Computed]
        public List<ApiResourceSecret> Secrets { get; set; }
        [Computed]
        public List<ApiResourceScope> Scopes { get; set; }
        [Computed]
        public List<ApiResourceClaim> UserClaims { get; set; }
        [Computed]
        public List<ApiResourceProperty> Properties { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }
}
