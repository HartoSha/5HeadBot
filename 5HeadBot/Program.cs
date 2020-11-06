using _5HeadBot.Modules.Internal;
using _5HeadBot.Services;
using _5HeadBot.Services.BotMessageService;
using _5HeadBot.Services.MemeService;
using _5HeadBot.Services.PictureService;
using _5HeadBot.Services.PictureService.Interfaces;
using _5HeadBot.Services.PictureService.PictureProviders;
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

            await services.GetRequiredService<ConfigService>().InitializeAsync();

            await services.GetRequiredService<DiscordConnectionService>().InitializeAsync();
            await services.GetRequiredService<BrowserService>().InitializeAsync();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await services.GetRequiredService<MusicService>().InitializeAsync();
            
            var config = services.GetRequiredService<LavaConfig>();
            await LogAsync(new LogMessage(LogSeverity.Info, "Main", $"Initialized services..."));
            await LogAsync(new LogMessage(LogSeverity.Info, "Main", $"Connecting to lavalink on: {config.Hostname}:{config.Port}..."));
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
                .AddSingleton<ConfigService>()
                .AddSingleton<DiscordConnectionService>()
                .AddSingleton<BotMessageSender>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<MessageSenderModuleBase>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PictureService>()
                .AddSingleton<ICatImageProvider, TheCatApi>()
                .AddSingleton<SearchService>()
                .AddSingleton<RNGService>()
                .AddSingleton<BrowserService>()
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
                .BuildServiceProvider();
        }
    }
}