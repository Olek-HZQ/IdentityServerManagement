// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using IdentityServer.Admin.Core.Entities.Clients;
using IdentityServer.Admin.Models.Client;

namespace IdentityServer.Admin.Infrastructure.Mappers
{
    /// <summary>
    /// Defines entity/model mapping for clients.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ClientMapperProfile : Profile
    {
        /// <summary>
        /// <see>
        ///     <cref>{ClientMapperProfile}</cref>
        /// </see>
        /// </summary>
        public ClientMapperProfile()
        {
            CreateMap<Client, ClientModel>().ReverseMap();

            CreateMap<ClientSecret, ClientSecretModel>().ReverseMap();

            CreateMap<ClientGrantType, ClientGrantTypeModel>().ReverseMap();

            CreateMap<ClientRedirectUri, ClientRedirectUriModel>().ReverseMap();

            CreateMap<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriModel>().ReverseMap();

            CreateMap<ClientScope, ClientScopeModel>().ReverseMap();

            CreateMap<ClientIdPRestriction, ClientIdPRestrictionModel>().ReverseMap();

            CreateMap<ClientClaim, ClientClaimModel>().ReverseMap();

            CreateMap<ClientCorsOrigin, ClientCorsOriginModel>().ReverseMap();

            CreateMap<ClientProperty, ClientPropertyModel>().ReverseMap();
        }
    }
}