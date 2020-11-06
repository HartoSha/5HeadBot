using _5HeadBot.Services.BotMessageService;
using _5HeadBot.Services.BotMessageService.Data;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;

namespace _5HeadBot.Services
{
    public class MusicService
    {
        private readonly LavaNode _lavaNode;
        private readonly DiscordSocketClient _discord;
        public MusicService(LavaNode lavaNode, DiscordSocketClient discord)
        {
            this._lavaNode = lavaNode;
            this._discord = discord;
        }
        private LavaNode ConnectedLavaNode
        {
            get => _lavaNode.IsConnected ? _lavaNode : throw new LavaNodeIsNotConnectedException();
        }
        public Task InitializeAsync()
        {
            _discord.Ready += async () => await _lavaNode.ConnectAsync();
            return Task.CompletedTask;
        }
        public async Task JoinAsync(IVoiceChannel channel)
        {
            await ConnectedLavaNode.JoinAsync(channel);
        }
        public async Task LeaveAsync(IVoiceChannel channel)
        {

            await ConnectedLavaNode.LeaveAsync(channel);
        }
        public async Task<BotMessageBuilder> PlayAsync(string searchQuery, IGuild guild)
        {
            if(ConnectedLavaNode.TryGetPlayer(guild, out var player))
            {
                var searchResponse = await ConnectedLavaNode.SearchYouTubeAsync(searchQuery);
                if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
                    searchResponse.LoadStatus == LoadStatus.NoMatches)
                {
                    return new BotMessageBuilder().
                        WithEmbedWithTitle($"I wasn't able to find anything for `{searchQuery}`.").
                        WithDisplayType(BotMessageStyle.Error);
                }
                var track = searchResponse.Tracks.FirstOrDefault();
                await player.PlayAsync(track);
                return new BotMessageBuilder().
                    WithEmbedWithTitle($"Now Playing: {track.Title}").
                    WithDisplayType(BotMessageStyle.Success);
            }
            return null;
        }

        public async Task<BotMessageBuilder> SkipAsync(IGuild guild)
        {
            if (ConnectedLavaNode.TryGetPlayer(guild, out var player))
            {
                var title = player.Track.Title;
                await player.SkipAsync();
                return
                    new BotMessageBuilder()
                    .WithEmbedWithTitle($"Scipped: {title}");
            }
            return new BotMessageBuilder()
                 .WithEmbedWithTitle($"Player not found.")
                 .WithDisplayType(BotMessageStyle.Exception);
        }
        private class LavaNodeIsNotConnectedException : Exception
        {
            public LavaNodeIsNotConnectedException() : base("Lava node server is not connected.") { }
        }
    }
}
