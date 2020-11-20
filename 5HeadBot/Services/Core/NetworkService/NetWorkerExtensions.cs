using Microsoft.Extensions.DependencyInjection;
using System;

namespace _5HeadBot.Services.Core.NetworkService
{
    public static class NetWorkerExtensions
    {
        /// <summary>
        /// Adds configured <see cref="NetWorker"/> 
        /// and its configuration <see cref="NetWorkerConfig"/>
        /// to the service collection
        /// </summary>
        public static IServiceCollection AddNetWorker(
            this IServiceCollection services, 
            Action<NetWorkerConfig> configOptions)
        {
            // configure the config
            var config = new NetWorkerConfig();
            configOptions?.Invoke(config);

            return services
                // add config
                .AddSingleton(config)
                
                // add NetWorker
                .AddSingleton<NetWorker>();
        }
    }
}
