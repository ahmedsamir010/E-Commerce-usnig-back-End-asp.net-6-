using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Talabat.Core.Repositories;
using Talabat.Service;

namespace API_01.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        private readonly IResponseCacheService _responseCacheService;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
            _responseCacheService = new ResponseCacheService(); // Instantiate your response cache service implementation here
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheKey = GenerateCacheKey(context);

            var cachedResponse = await _responseCacheService.GetCachedResponse(cacheKey);
            if (cachedResponse != null)
            {
                context.Result = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult result && result.StatusCode == 200)
            {
                await _responseCacheService.CacheResponseAsync(cacheKey, result.Value.ToString(), TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(context.HttpContext.Request.Path);

            foreach (var (key, value) in context.ActionArguments.OrderBy(a => a.Key))
            {
                keyBuilder.Append($"|{key}:{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
