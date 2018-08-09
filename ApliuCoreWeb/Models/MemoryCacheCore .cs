using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApliuCoreWeb.Models
{
    public class MemoryCacheCore
    {
        private static IMemoryCache Cache { get; set; } = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 缓存指定对象
        /// </summary>
        public static void SetValue(String key, Object value)
        {
            Cache.Set(key, value);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        public static Object GetValue(String key)
        {
            Object value = Cache.Get(key);
            return value;
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void Remove(String key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 判断缓存中是否含有缓存该键
        /// </summary>
        public static Boolean Exists(string key)
        {
            Boolean isExists = GetValue(key) != null;
            return isExists;
        }
    }
}
