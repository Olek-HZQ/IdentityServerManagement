using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Dapper.Repositories.CommonInterfaces;
using IdentityServer4.Stores;
using Serilog;
using PersistedGrant = IdentityServer4.Models.PersistedGrant;

namespace IdentityServer.Admin.Services.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistedGrantRepository _repository;

        public PersistedGrantStore(IPersistedGrantRepository repository)
        {
            _repository = repository;
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            var existsGrant = await _repository.GetByKeyAsync(grant.Key);

            var persistedGrant = new Core.Entities.PersistedGrant
            {
                Key = grant.Key,
                Type = grant.Type,
                SubjectId = grant.SubjectId,
                SessionId = grant.SessionId,
                ClientId = grant.ClientId,
                Description = grant.Description,
                CreationTime = grant.CreationTime,
                Expiration = grant.Expiration,
                ConsumedTime = grant.ConsumedTime,
                Data = grant.Data
            };

            try
            {
                if (existsGrant == null)
                {
                    Log.Debug("{persistedGrantKey} not found in database", grant.Key);
                    await _repository.InsertAsync(persistedGrant);
                }
                else
                {
                    Log.Debug("{persistedGrantKey} found in database", grant.Key);
                    await _repository.UpdateAsync(persistedGrant);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"PersistedGrantStore >> StoreAsync Error: {ex.Message}");
            }
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var existsGrant = await _repository.GetByKeyAsync(key);

            var grant = new PersistedGrant();

            if (existsGrant != null)
            {
                grant.Key = existsGrant.Key;
                grant.Type = existsGrant.Type;
                grant.SubjectId = existsGrant.SubjectId;
                grant.SessionId = existsGrant.SessionId;
                grant.ClientId = existsGrant.ClientId;
                grant.Description = existsGrant.Description;
                grant.CreationTime = existsGrant.CreationTime;
                grant.Expiration = existsGrant.Expiration;
                grant.ConsumedTime = existsGrant.ConsumedTime;
                grant.Data = existsGrant.Data;
            }

            return grant;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var grants = (await _repository.GetListBySubjectIdAsync(filter.SubjectId)).ToList();

            if (grants.Any())
            {
                return grants.Select(x => new PersistedGrant
                {
                    Key = x.Key,
                    Type = x.Type,
                    SubjectId = x.SubjectId,
                    SessionId = x.SessionId,
                    ClientId = x.ClientId,
                    Description = x.Description,
                    CreationTime = x.CreationTime,
                    Expiration = x.Expiration,
                    ConsumedTime = x.ConsumedTime,
                    Data = x.Data
                });
            }

            return new List<PersistedGrant>();
        }

        public async Task RemoveAsync(string key)
        {
            await _repository.DeleteByKeyAsync(key);
        }

        public async Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            await _repository.DeleteAllBySubjectIdAsync(filter.SubjectId);
        }
    }
}
