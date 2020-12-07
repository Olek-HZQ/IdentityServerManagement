// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable 1591

namespace IdentityServer.Admin.Core.Entities.ApiResource
{
    [Table("ApiResourceClaims")]
    public class ApiResourceClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public int ApiResourceId { get; set; }
    }
}