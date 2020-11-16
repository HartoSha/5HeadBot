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
            return services.AddSingleton<ICachingService, T>();
        }
    }
}
