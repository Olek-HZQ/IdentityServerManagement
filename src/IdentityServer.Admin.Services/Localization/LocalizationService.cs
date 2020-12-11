using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Dtos.Localization;
using IdentityServer.Admin.Core.Entities.Localization;
using IdentityServer.Admin.Dapper.Repositories.Localization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;

namespace IdentityServer.Admin.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ILocalizationRepository _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IWorkContext _workContext;

        public LocalizationService(ILocalizationRepository repository, IMemoryCache memoryCache, IWorkContext workContext)
        {
            _repository = repository;
            _memoryCache = memoryCache;
            _workContext = workContext;
        }

        /// <summary>
        /// Prepare cache entry options for the passed key
        /// </summary>
        /// <param name="cacheTime"></param>
        /// <returns>Cache entry options</returns>
        private static MemoryCacheEntryOptions PrepareEntryOptions(int? cacheTime = null)
        {
            //set expiration time for the passed cache key
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime ?? CachingConstant.CacheTime)
            };

            return options;
        }

        private void RemoveCachedLanguage(int languageId)
        {
            string key = string.Format(LocalizationDefaults.LocaleStringResourcesAllCacheKey, languageId);
            if (_memoryCache.TryGetValue(key, out _))
            {
                _memoryCache.Remove(key);
            }
        }

        public async Task<Dictionary<string, KeyValuePair<int, string>>> GetAllResourceValuesAsync(int languageId)
        {
            var key = string.Format(LocalizationDefaults.LocaleStringResourcesAllCacheKey, languageId);

            var result = await _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(PrepareEntryOptions());
                var resources = (await _repository.GetResourcesByLanguageIdAsync(languageId)).ToList();
                Dictionary<string, KeyValuePair<int, string>> dictionary = new Dictionary<string, KeyValuePair<int, string>>();
                resources.ForEach(x =>
                {
                    if (!dictionary.ContainsKey(x.ResourceName))
                    {
                        dictionary.Add(x.ResourceName, new KeyValuePair<int, string>(x.Id, x.ResourceValue));
                    }
                });
                return dictionary;
            });

            return result;
        }

        public async Task<PagedLocalStringResourceDto> GetPagedAsync(int languageId, string search, int page, int pageSize = PageConstant.PageSize)
        {
            return await _repository.GetPagedAsync(languageId, search, page, pageSize);
        }

        public async Task<LocaleStringResource> GetStringResourceByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _repository.GetAsync(id);
        }

        public async Task<string> GetResourceAsync(string resourceKey, bool logIfNotFound = true, string defaultValue = "")
        {
            int languageId = _workContext.WorkingLanguage.Id;
            //string result;
            //string key = string.Format(LocalizationDefaults.LocaleStringResourcesAllCacheKey, languageId);
            //if (_memoryCache.TryGetValue(key, out Dictionary<string, KeyValuePair<int, string>> resources))
            //{
            //    var cachedResource = resources.FirstOrDefault(x => x.Key == resourceKey);
            //    result = cachedResource.Value.Value;
            //}
            //else
            //{
            //    var cachedAllResourceValues = await GetAllResourceValuesAsync(languageId); // 缓存不存在或者已过期，重新添加

            //    var cachedResource = cachedAllResourceValues.FirstOrDefault(x => x.Key == resourceKey);
            //    result = cachedResource.Value.Value;
            //}

            var result = (await _repository.GetResourceAsync(resourceKey, languageId))?.ResourceValue;

            if (!string.IsNullOrEmpty(result))
                return result;

            if (logIfNotFound)
                Log.Information($"Resource string ({resourceKey}) is not found. Language ID = {languageId}");

            if (!string.IsNullOrEmpty(defaultValue))
            {
                result = defaultValue;
            }

            return result;
        }

        public async Task<int> InsertStringResourceAsync(LocaleStringResource resource)
        {
            var result = await _repository.InsertAsync(resource);
            if (result > 0)
            {
                RemoveCachedLanguage(resource.LanguageId);
            }
            return result;
        }

        public async Task<bool> UpdateStringResourceAsync(LocaleStringResource resource)
        {
            var result = await _repository.UpdateAsync(resource);
            if (result)
            {
                RemoveCachedLanguage(resource.LanguageId);
            }

            return result;
        }

        public async Task<bool> DeleteStringResourceAsync(LocaleStringResource resource)
        {
            var result = await _repository.DeleteAsync(resource);
            if (result)
            {
                RemoveCachedLanguage(resource.LanguageId);
            }

            return result;
        }

        public async Task<bool> SaveResourcesAsync(int languageId, string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return false;

                var lsNamesList =
                    (await _repository.GetResourcesByLanguageIdAsync(languageId)).ToDictionary(x => x.ResourceName, y => y);

                var lrsToUpdateList = new List<LocaleStringResource>();
                var lrsToInsertList = new Dictionary<string, LocaleStringResource>();

                var allResources = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                if (allResources.Any())
                {
                    foreach (var item in allResources)
                    {
                        if (lsNamesList.ContainsKey(item.Key))
                        {
                            var lsr = lsNamesList[item.Key];
                            lsr.ResourceValue = item.Value;
                            lrsToUpdateList.Add(lsr);
                        }
                        else
                        {
                            var lsr = new LocaleStringResource { LanguageId = languageId, ResourceName = item.Key, ResourceValue = item.Value };
                            if (lrsToInsertList.ContainsKey(item.Key))
                                lrsToInsertList[item.Key] = lsr;
                            else
                                lrsToInsertList.Add(item.Key, lsr);
                        }
                    }

                    return await _repository.SaveResourcesAsync(languageId, lrsToInsertList.Values.ToList(), lrsToUpdateList);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           

            return true;
        }
    }
}
