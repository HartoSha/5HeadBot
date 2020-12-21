using System;

namespace _5HeadBot.Services.Core.CachingService
{
    public interface ICachingService
    {
        /// <summary>
        /// Receives the value corresponding 
        /// to the <paramref name="key"/> from the cache
        /// </summary>
        /// <remarks><see langword="null"/> if no such value exists</remarks>
        /// <param name="key">A key referring to the value</param>
        /// <returns>Value from the cache or <see langword="null"/></returns>
        public object Get(string key);
        
        /// <summary>
        /// Sets (<paramref name="key"/> : <paramref name="value"/>) pair in the cache.
        /// The pair expires at <paramref name="absExpiration"/> and is being deleted
        /// </summary>
        /// <param name="key">Key to refer the value</param>
        /// <param name="value">Value to be set</param>
        /// <param name="absExpiration">
        /// A date the <paramref name="key"/> : <paramref name="value"/> pair will be deleted
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>Setted value</returns>
        public object Set(string key, object value, DateTimeOffset absExpiration);
    }
}
