using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Repositories
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, string response, TimeSpan timeToLive);
        Task<string> GetCachedResponse(string cacheKey);
    }
}
