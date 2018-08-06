using System;
using System.Collections;

namespace ApliuTools.WebTools
{
    public static class HttpRuntimeCache
    {
        /// <summary>
        /// 设置缓存时间，配置(从配置文件中读取)
        /// </summary>
        private const double Seconds = 30 * 24 * 60 * 60;

        /// <summary>
        /// 缓存指定对象，设置缓存
        /// </summary>
        public static bool Add(string key, object value)
        {
            return Add(key, value, null, DateTime.Now.AddSeconds(Seconds), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        /// <summary>
        ///  缓存指定对象，设置缓存
        /// </summary>
        public static bool Add(string key, object value, string path)
        {
            try
            {
                var cacheDependency = new CacheDependency(path);
                return Add(key, value, cacheDependency);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 缓存指定对象，设置缓存
        /// </summary>
        public static bool Add(string key, object value, CacheDependency cacheDependency)
        {
            return Add(key, value, cacheDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        /// <summary>
        /// 缓存指定对象，设置缓存
        /// </summary>
        public static bool Add(string key, object value, double seconds, bool isAbsulute)
        {
            return Add(key, value, null, (isAbsulute ? DateTime.Now.AddSeconds(seconds) : Cache.NoAbsoluteExpiration),
                (isAbsulute ? Cache.NoSlidingExpiration : TimeSpan.FromSeconds(seconds)), CacheItemPriority.Default, null);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        public static object Get(string key)
        {
            return GetPrivate(key);
        }

        /// <summary>
        /// 判断缓存中是否含有缓存该键
        /// </summary>
        public static bool Exists(string key)
        {
            return (GetPrivate(key) != null);
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            HttpRuntime.Cache.Remove(key);
            return true;
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns></returns>
        public static bool RemoveAll()
        {
            IDictionaryEnumerator iDictionaryEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (iDictionaryEnumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove(Convert.ToString(iDictionaryEnumerator.Key));
            }
            return true;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        public static bool Add(string key, object value, CacheDependency cacheDependency, DateTime dateTime,
            TimeSpan timeSpan, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback cacheItemRemovedCallback)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return false;
            }
            HttpRuntime.Cache.Insert(key, value, cacheDependency, dateTime, timeSpan, cacheItemPriority, cacheItemRemovedCallback);
            return true;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        private static object GetPrivate(string key)
        {
            return string.IsNullOrEmpty(key) ? null : HttpRuntime.Cache.Get(key);
        }
    }
}
