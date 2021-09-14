using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_core_cache.Controllers
{
    public static class CacheKeys
    {
        public static string Entry => "_Entry";
    }

    [ApiController]
    [Route("[controller]")]
    public class InMemoryCacheController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("demo1")]
        public IActionResult Demo1()
        {
            var datetime = _memoryCache.GetOrCreate(
                CacheKeys.Entry, 
                entry => { return DateTime.Now; }
                );
            
            return Ok(datetime);
        }

        [HttpGet("demo2")]
        public IActionResult Demo2()
        {
            DateTime cacheEntry;

            // Look for cache key.
            if (!_memoryCache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache.
                _memoryCache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            return Ok(cacheEntry);
        }
    }
}
