using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApliuCoreWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        private readonly IMemoryCache IMemoryCache;

        public CacheController(IMemoryCache iMemoryCache)=> IMemoryCache = iMemoryCache;

        [HttpPost]
        public void ChatLogin()
        {
            string connectionId = HttpContext.Request.Form["ConnectionId"];
            string userName = HttpContext.Request.Form["UserName"];
            IMemoryCache.Set(connectionId, userName);
        }
    }
}
