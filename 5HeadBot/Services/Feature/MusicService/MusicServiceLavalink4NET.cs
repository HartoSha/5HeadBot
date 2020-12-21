using _5HeadBot.Services.Feature.MusicService.Data;
using Discord;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.MusicService
{
    public class MusicServiceLavalink4NET : IMusicService
    {
        private readonly IAudioService _audio;
        public MusicServiceLavalink4NET(IAudioService audio, DiscordSocketClient discord)
        {
            _audio = audio;
            discord.Ready += async () =>
            {
                await _audio.InitializeAsync();
            };
        }
        public async Task JoinAsync(IVoiceChannel channel)
        {
            await GetPlayerAsync(channel, shouldJoin: true);
        }

        public async Task LeaveAsync(IVoiceChannel channel)
        {
            var player = await GetPlayerAsync(channel, false);
            if (player != null)
                await player.StopAsync(true);
        }

        public async Task<MusicTrackInfo> PlayAsync(string query, IVoiceChannel voiceChannel, bool enqueue = true)
        {
            if (voiceChannel is null) 
                return null;

            var foundTrack = await SearchForTrackAsync(query);
            if (foundTrack is null) 
                return null;

            var player = await GetPlayerAsync(voiceChannel);
            
            if (enqueue)
                await player.PlayTopAsync(foundTrack);
            else
                await player.PlayAsync(foundTrack);

            return foundTrack.AsMusicTrackInfo();
        }

        public async Task<MusicTrackInfo> GetCurrentAsync(IVoiceChannel voiceChannel)
        {
            var player = await GetPlayerAsync(voiceChannel);
            return player?.CurrentTrack?.AsMusicTrackInfo();
        }

        public async Task<IImmutableQueue<MusicTrackInfo>> GetQueueAsync(IVoiceChannel voiceChannel)
        {
            var queue = new Queue<MusicTrackInfo>();
            var player = await GetPlayerAsync(voiceChannel);
            foreach (var track in player?.Queue)
            {
                queue.Enqueue(track.AsMusicTrackInfo());
            }
            return ImmutableQueue.CreateRange(queue);
        }
        public async Task<MusicTrackInfo> SkipAsync(IVoiceChannel voiceChannel)
        {
            var player = await GetPlayerAsync(voiceChannel);
            MusicTrackInfo skipedTrack = player?.CurrentTrack?.AsMusicTrackInfo();
            await player.SkipAsync();
            return skipedTrack;
        }
        private async Task<VoteLavalinkPlayer> GetPlayerAsync(IVoiceChannel channel, bool shouldJoin = false)
        {
            if (channel is null)
                return null;

            VoteLavalinkPlayer player;

            if (shouldJoin)
                player = await _audio.JoinAsync<VoteLavalinkPlayer>(channel.GuildId, channel.Id);
            else
                player = _audio.GetPlayer<VoteLavalinkPlayer>(channel.GuildId);

            return player;
        }
        private async Task<LavalinkTrack> SearchForTrackAsync(string trackName)
        {
            var track =
                await _audio.GetTrackAsync(trackName, Lavalink4NET.Rest.SearchMode.YouTube) ??
                await _audio.GetTrackAsync(trackName, Lavalink4NET.Rest.SearchMode.SoundCloud);
            return track;
        }
    }
}
