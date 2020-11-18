using Microsoft.Extensions.DependencyInjection;

namespace _5HeadBot.Services.Core.CachingService
{
    /// <summary>
    /// Helps adding cahing to the app
    /// </summary>
    public static class CachingExtensions
    {
        public static IServiceCollection UseCaching<T>(this IServiceCollection services) where T : class, ICachingService
        {
            // Only add memory cache if caching is being done by MemoryCachingService
            // this is just to simplify clients' UseCaching usage.
            
            // Because client might forget to use default ASP.NET's .AddMemoryCache()
            // when trying to use MemoryCachingService as a cache provider
            // (as i did) and get unexpected exceptions.
            if (typeof(T) == typeof(MemoryCachingService))
                services.AddMemoryCache();

            return services.AddSingleton<ICachingService, T>();
        }
    }
}
