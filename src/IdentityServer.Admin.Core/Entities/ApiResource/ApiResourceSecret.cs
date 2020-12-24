// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

#pragma warning disable 1591

namespace IdentityServer.Admin.Core.Entities.ApiResource
{
    [Table(TableNameConstant.ApiResourceSecret)]
    public class ApiResourceSecret : Secret
    {
        public int ApiResourceId { get; set; }

        [Computed]
        public virtual ApiResource ApiResource { get; set; }
    }
}