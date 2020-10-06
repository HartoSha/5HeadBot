using Discord;
using Discord.WebSocket;
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
            _lavaNode = lavaNode;
            _discord = discord;
        }
        public Task InitializeAsync()
        {
            _discord.Ready += OnDiscordReady;
            return Task.CompletedTask;
        }
        private async Task OnDiscordReady()
        {
            if (!_lavaNode.IsConnected)
                await _lavaNode.ConnectAsync();
        }
        public async Task JoinAsync(IVoiceChannel channel)
        {
            if (_lavaNode.IsConnected) 
            {
                await _lavaNode.JoinAsync(channel);
            }
        }
        public async Task LeaveAsync(IVoiceChannel channel)
        {
            if (_lavaNode.IsConnected)
            {
                await _lavaNode.LeaveAsync(channel);
            }
        }
        public async Task<string> PlayAsync(string searchQuery, IGuild guild)
        {
            if (_lavaNode.TryGetPlayer(guild, out var player))
            {
                var searchResponse = await _lavaNode.SearchYouTubeAsync(searchQuery);
                if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
                    searchResponse.LoadStatus == LoadStatus.NoMatches)
                {
                    return $"I wasn't able to find anything for `{searchQuery}`.";
                }
                var track = searchResponse.Tracks.FirstOrDefault();
                await player.PlayAsync(track);
                return $"Now Playing: {track.Title}";
            }
            else
            {
                return "You need to have a player first";
            }
        }
    }
}
