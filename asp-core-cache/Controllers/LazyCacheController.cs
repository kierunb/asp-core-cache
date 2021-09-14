using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_core_cache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LazyCacheController : ControllerBase
    {
        private readonly ILogger<LazyCacheController> _logger;
        private readonly IAppCache _cache;

        public LazyCacheController(
            ILogger<LazyCacheController> logger,
            IAppCache cache
            )
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("")]
        public async Task<IActionResult> Test1()
        {
            Func<Task<DateTime>> expensiveItem = () => Task.FromResult(DateTime.Now);

            var cachedData = await _cache.GetOrAddAsync(
                key: "key", 
                addItemFactory: expensiveItem, 
                expires: DateTimeOffset.Now.AddSeconds(5));
            return Ok(cachedData);
        }

        private async Task<List<string>> GetData()
        {
            return await Task.FromResult(new List<string>()
                {
                   "John Smith",
                   "Steve Smith",
                   "Rick Smith"
                });
        }
    }
}
