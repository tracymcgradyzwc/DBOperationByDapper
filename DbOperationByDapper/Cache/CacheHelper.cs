using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbOperationByDapper.Cache
{
    /// <summary>
    /// Core版本缓存
    /// </summary>
    public class CacheHelper
    {
        static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        public static T Get<T>(string key) where T : class
        {
            T val = null;
            if (key != null && cache.TryGetValue(key, out val))
            {
                return val;
            }
            else
            {
                return default(T);
            }
        }

        public static void Set(string key, object val, int seconds)
        {

            if (key != null)
            {
                cache.Set(key, val, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(seconds)
                });
            }
        }

        public static void Set(string key, object val)
        {

            if (key != null)
            {
                cache.Set(key, val);
            }
        }

        public static void Remove(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        public static void Flush()
        {
            cache.Dispose();
        }
    }
}
