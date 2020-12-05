﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#pragma warning disable 1591

namespace IdentityServer.Admin.Models
{
    public class ClientRedirectUriModel
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }

        public int ClientId { get; set; }
    }
}