using System;

namespace _5HeadBot.Services.Core.CachingService
{
    public interface ICachingService
    {
        public object GetValue(string key);
        public object Add(string key, object value, DateTimeOffset absExpiration);
    }
}
