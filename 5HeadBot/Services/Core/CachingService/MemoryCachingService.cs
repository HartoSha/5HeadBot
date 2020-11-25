using Microsoft.Extensions.Caching.Memory;
using System;

namespace _5HeadBot.Services.Core.CachingService
{
    /// <summary>
    /// Class used to store cached values in the memory
    /// </summary>
    public class MemoryCachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public object Set(string key, object value, DateTimeOffset absExpiration)
        {
            if (absExpiration < DateTimeOffset.Now)
                throw new ArgumentOutOfRangeException(nameof(absExpiration), "Date already expired");

            return _memoryCache.Set(key, value, absExpiration);
        }
    }
}
