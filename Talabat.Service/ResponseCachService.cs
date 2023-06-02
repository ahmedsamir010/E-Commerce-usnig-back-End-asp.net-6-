using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Talabat.Core.Repositories;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private static readonly ConcurrentDictionary<string, CacheItem> Cache = new ConcurrentDictionary<string, CacheItem>();

        public Task CacheResponseAsync(string cacheKey, string response, TimeSpan timeToLive)
        {
            var cacheItem = new CacheItem(response, DateTime.UtcNow.Add(timeToLive));
            Cache.AddOrUpdate(cacheKey, cacheItem, (_, __) => cacheItem);

            return Task.CompletedTask;
        }

        public Task<string> GetCachedResponse(string cacheKey)
        {
            if (Cache.TryGetValue(cacheKey, out var cacheItem) && cacheItem.Expiration > DateTime.UtcNow)
            {
                return Task.FromResult(cacheItem.Response);
            }

            return Task.FromResult<string>(null);
        }

        private class CacheItem
        {
            public string Response { get; }
            public DateTime Expiration { get; } 

            public CacheItem(string response, DateTime expiration)
            {
                Response = response;
                Expiration = expiration;
            }
        }
    }
}
