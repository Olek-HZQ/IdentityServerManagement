// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#pragma warning disable 1591

namespace IdentityServer.Admin.Models.ApiResource
{
    public class ApiResourcePropertyModel : PropertyModel
    {
        public int ApiResourceId { get; set; }
        public string ApiResourceName { get; set; }
    }
}