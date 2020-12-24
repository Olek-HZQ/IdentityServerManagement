// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

#pragma warning disable 1591

namespace IdentityServer.Admin.Core.Entities.Clients
{
    [Table(TableNameConstant.ClientIdPRestriction)]
    public class ClientIdPRestriction
    {
        public int Id { get; set; }
        public string Provider { get; set; }

        public int ClientId { get; set; }

        [Computed]
        public virtual Client Client { get; set; }
    }
}