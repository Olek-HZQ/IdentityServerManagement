﻿using System.Threading.Tasks;
using IdentityServer.Admin.Core.Data;
using IdentityServer.Admin.Core.Dtos;
using IdentityServer.Admin.Core.Entities;

namespace IdentityServer.Admin.Dapper.Repositories.CommonInterfaces
{
    public interface IClientSecretRepository : IRepository<ClientSecret>
    {
        Task<PagedClientSecretDto> GetPagedAsync(int clientId, int page, int pageSize);
    }
}
