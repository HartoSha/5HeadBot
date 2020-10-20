using _5HeadBot.Modules.Internal;
using _5HeadBot.Services;
using _5HeadBot.Services.BotMessageService;
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
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.

            await using var services = ConfigureServices();

            services.GetRequiredService<DiscordSocketClient>().Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;
            services.GetRequiredService<LavaNode>().OnLog += LogAsync;

            await services.GetRequiredService<ConfigService>().
                InitializeAsync(
                    configJsonPath: "config.json",
                    searchDepth: 10
                );
            await services.GetRequiredService<DiscordConnectionService>().InitializeAsync();
            await services.GetRequiredService<BrowserService>().InitializeAsync();
            await services.GetRequiredService<MusicService>().InitializeAsync();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
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
                .AddTransient<BotMessageBuilder>()
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
                .AddSingleton<LavaConfig>()
                .AddLavaNode(x => {
                    x.SelfDeaf = false;
                })
                .AddSingleton<MusicService>()
                .BuildServiceProvider();
        }
    }
}