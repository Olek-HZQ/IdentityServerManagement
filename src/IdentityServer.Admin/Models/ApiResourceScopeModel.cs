﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

namespace IdentityServer.Admin.Models
{
    public class ApiResourceScopeModel
    {
        public int Id { get; set; }
        public string Scope { get; set; }

        public int ApiResourceId { get; set; }
        public string ApiResourceName { get; set; }
    }
}