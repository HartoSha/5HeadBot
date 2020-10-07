using _5HeadBot.Services;
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
    static class LavaNodeExtintions
    {
        public static void Dispose(this LavaNode node)
        {
            Task.Run(async () =>
            {
                await node.DisconnectAsync();
            });
        }
    }
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

            var services = ConfigureServices();

            var client = services.GetRequiredService<DiscordSocketClient>();
            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;
            services.GetRequiredService<LavaNode>().OnLog += LogAsync;

            // maybe move logic of if-esle below to InitializeAsync() of ConfigService?
            await services.GetRequiredService<ConfigService>().
                InitializeAsync(
                    configJsonPath: "config.json", 
                    searchDepth: 10
                );

            var config = services.GetRequiredService<ConfigService>().Config;

            if (!string.IsNullOrWhiteSpace(config.DiscordToken))
            {
                await client.LoginAsync(TokenType.Bot, config.DiscordToken);
                await client.StartAsync();

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
                await services.GetRequiredService<MusicService>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
            else
            {
                await LogAsync(new LogMessage(LogSeverity.Error, config.DiscordToken, "config.json does not exist"));
            }

            await services.DisposeAsync();
        }
        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ConfigService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<HttpClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<PictureService>()
                .AddSingleton<ICatImageProvider, TheCatApi>()
                .AddSingleton<SearchService>()
                .AddSingleton<RNGService>()
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