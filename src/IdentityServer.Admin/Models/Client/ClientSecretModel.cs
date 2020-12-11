// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer.Admin.Core.Entities.Enums;

#pragma warning disable 1591

namespace IdentityServer.Admin.Models.Client
{
    public class ClientSecretModel : SecretModel
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }
        public string HashType { get; set; }
        public HashType HashTypeEnum => Enum.TryParse(HashType, true, out HashType result) ? result : Core.Entities.Enums.HashType.Sha256;
    }
}