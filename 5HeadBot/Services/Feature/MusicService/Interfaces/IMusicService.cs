using _5HeadBot.Services.Feature.MusicService.Data;
using Discord;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.MusicService
{
    public interface IMusicService
    {
        /// <summary>
        /// Joins the specific <paramref name="voiceChannel"/>
        /// </summary>
        Task JoinAsync(IVoiceChannel voiceChannel);
        
        /// <summary>
        /// Leaves the specific <paramref name="voiceChannel"/>
        /// </summary>
        Task LeaveAsync(IVoiceChannel voiceChannel);

        /// <summary>
        /// Plays track found by <paramref name="query"/> for the <paramref name="voiceChannel"/>
        /// </summary>
        /// <returns>Found track or <see langword="null"/></returns>
        Task<MusicTrackInfo> PlayAsync(string query, IVoiceChannel voiceChannel, bool enqueue = true);

        /// <summary>
        /// Gets current playing track for the <paramref name="voiceChannel"/>
        /// </summary>
        /// <returns>Current playing track or <see langword="null"/></returns>
        Task<MusicTrackInfo> GetCurrentAsync(IVoiceChannel voiceChannel);

        /// <summary>
        /// Gets enqueued tracks for the <paramref name="voiceChannel"/>
        /// </summary>
        Task<IImmutableQueue<MusicTrackInfo>> GetQueueAsync(IVoiceChannel voiceChannel);

        /// <summary>
        /// Skips track in a <paramref name="voiceChannel"/>
        /// </summary>
        /// <returns>Skipped track or <see langword="null"/></returns>
        Task<MusicTrackInfo> SkipAsync(IVoiceChannel voiceChannel);
    }
}
