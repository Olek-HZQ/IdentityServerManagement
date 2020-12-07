// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Dapper.Contrib.Extensions;

#pragma warning disable 1591

namespace IdentityServer.Admin.Core.Entities.Clients
{
    [Table("ClientRedirectUris")]
    public class ClientRedirectUri
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }

        public int ClientId { get; set; }
    }
}