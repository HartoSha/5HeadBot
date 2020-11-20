using _5HeadBot.Services.Core;
using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.CachingService;
using _5HeadBot.Services.Core.ConfigService;
using _5HeadBot.Services.Core.NetworkService;
using _5HeadBot.Services.Core.NetworkService.Deserializers;
using _5HeadBot.Services.Feature;
using _5HeadBot.Services.Feature.MemeService;
using _5HeadBot.Services.Feature.PictureService;
using _5HeadBot.Services.Feature.PictureService.Interfaces;
using _5HeadBot.Services.Feature.PictureService.PictureProviders;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Victoria;

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

            services.GetRequiredService<DiscordSocketClient>().Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;
            services.GetRequiredService<LavaNode>().OnLog += LogAsync;

            await services.GetRequiredService<ConfigurationService>().InitializeAsync();

            await services.GetRequiredService<DiscordConnectionService>().InitializeAsync();
            await services.GetRequiredService<MusicService>().InitializeAsync();
            var config = services.GetRequiredService<LavaConfig>();
            await LogAsync(new LogMessage(LogSeverity.Info, "Main", $"Connecting to lavalink on: {config.Hostname}:{config.Port}..."));

            await services.GetRequiredService<NetWorker>().InitializeAsync();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await LogAsync(new LogMessage(LogSeverity.Info, "Main", $"Initialized services... Bot is usable now!"));
            
            await Task.Delay(Timeout.Infinite);
        }
        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
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
                .AddLavaNode(x =>
                {
                    x.Port = Environment.GetEnvironmentVariable("LAVALINK_PORT") is null ? (ushort)8080 : ushort.Parse(Environment.GetEnvironmentVariable("LAVALINK_PORT"));
                    x.Hostname = Environment.GetEnvironmentVariable("LAVALINK_HOSTNAME") ?? "localhost";
                    x.Authorization = Environment.GetEnvironmentVariable("LAVALINK_PASSWORD") ?? "youshallnotpass";
                    x.SelfDeaf = false;
                })
                .AddSingleton<MusicService>()
                .AddSingleton<MemeService>()
                .UseCaching<MemoryCachingService>()
                .BuildServiceProvider();
        }
    }
}