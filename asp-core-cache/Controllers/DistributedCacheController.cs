using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_core_cache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistributedCacheController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public string CachedTimeUTC { get; set; }

        public DistributedCacheController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            CachedTimeUTC = "Cached Time Expired";
            var encodedCachedTimeUTC = await _cache.GetAsync("cachedTimeUTC");

            if (encodedCachedTimeUTC != null)
            {
                CachedTimeUTC = Encoding.UTF8.GetString(encodedCachedTimeUTC);
            }

            return Ok(CachedTimeUTC);
        }

        [HttpGet("reset")]
        public async Task<IActionResult> Reset()
        {
            var currentTimeUTC = DateTime.UtcNow.ToString();
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);

            return Ok("time cached");
        }
    }
}
