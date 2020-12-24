using _5HeadBot.Services.Core;
using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.CachingService;
using _5HeadBot.Services.Core.ConfigService;
using _5HeadBot.Services.Core.LoggingService;
using _5HeadBot.Services.Core.NetworkService;
using _5HeadBot.Services.Core.NetworkService.Deserializers;
using _5HeadBot.Services.Feature;
using _5HeadBot.Services.Feature.MemeService;
using _5HeadBot.Services.Feature.MusicService;
using _5HeadBot.Services.Feature.MusicService.Logging;
using _5HeadBot.Services.Feature.PictureService;
using _5HeadBot.Services.Feature.PictureService.Interfaces;
using _5HeadBot.Services.Feature.PictureService.PictureProviders;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace _5HeadBot
{
    // Bootstraped with https://github.com/discord-net/Discord.Net/tree/dev/samples/02_commands_framework
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await using var services = ConfigureServices();

            // set up logging
            var logger = services.GetRequiredService<ILoggingService>();
            services.GetRequiredService<DiscordSocketClient>().Log += logger.LogAsync;
            services.GetRequiredService<CommandService>().Log += logger.LogAsync;

            // initialize services
            await services.GetRequiredService<ConfigurationService>().InitializeAsync();
            await services.GetRequiredService<DiscordConnectionService>().InitializeAsync();
            await services.GetRequiredService<NetWorker>().InitializeAsync();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await services.GetRequiredService<ILoggingService>()
                .LogAsync(new LogMessage(LogSeverity.Info, "Main", $"Initialized services... Bot is usable now!"));

            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ILoggingService, ConsoleLogger>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<ConfigurationService>()
                .AddSingleton<DiscordConnectionService>()
                .AddSingleton<BotMessageSender>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PictureService>()
                .AddSingleton<ICatImageProvider, TheCatApi>()
                .AddSingleton<SearchService>()
                .AddSingleton<RNGService>()
                .AddNetWorker(config =>
                {
                    config.DefaultDeserializer = new JsonDeserializer();
                })
                .AddSingleton<IstuService>()

                // TODO: Move configuration to Add... method
                // Lavalink4Net starts
                .AddSingleton<IAudioService, LavalinkNode>()
                .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
                .AddSingleton(new LavalinkNodeOptions
                {
                    RestUri = new UriBuilder()
                    {
                        Scheme = "http",
                        Host = Environment.GetEnvironmentVariable("LAVALINK_HOSTNAME") ?? "localhost",
                        Port = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAVALINK_PORT")) ? 8080 : int.Parse(Environment.GetEnvironmentVariable("LAVALINK_PORT")),
                    }.ToString(),
                    WebSocketUri = new UriBuilder()
                    {
                        Scheme = "ws",
                        Host = Environment.GetEnvironmentVariable("LAVALINK_HOSTNAME") ?? "localhost",
                        Port = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAVALINK_PORT")) ? 8080 : int.Parse(Environment.GetEnvironmentVariable("LAVALINK_PORT")),
                    }.ToString(),
                    Password = Environment.GetEnvironmentVariable("LAVALINK_PASSWORD") ?? "youshallnotpass",
                })
                .AddSingleton<Lavalink4NET.Logging.ILogger, Lavalink4NETLogger>()
                // Lavalink4Net ends

                .AddSingleton<IMusicService, MusicServiceLavalink4NET>()
                .AddSingleton<MemeService>()
                .UseCaching<MemoryCachingService>()
                .BuildServiceProvider();
        }
    }
}