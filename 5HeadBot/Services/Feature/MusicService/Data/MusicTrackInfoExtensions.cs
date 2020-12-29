using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using Lavalink4NET.Player;
using System.Text;

namespace _5HeadBot.Services.Feature.MusicService.Data
{
    public static class MusicTrackInfoExtensions
    {
        public static BotMessage AsBotMessage(this MusicTrackInfo trackInfo)
        {
            return trackInfo
                .AsBotMessageBuilder()
                .Build();
        }

        public static BotMessageBuilder AsBotMessageBuilder(this MusicTrackInfo trackInfo)
        {
            return new BotMessageBuilder()
                .WithEmbedAuthor(trackInfo.Author)
                .WithEmbedWithTitle(trackInfo.Title)
                .WithEmbedWithUrl(trackInfo.Url)
                .WithEmbedDescription(trackInfo.Duration.ToString(@"mm\:ss"));
        }

        /// <summary>
        /// Presents current track position like 00:00 --|----- 04:26
        /// </summary>
        public static string AsTimelineString(this MusicTrackInfo trackInfo)
        {
            var estematedTime = trackInfo.Position.TotalSeconds > 0 ? trackInfo.Position.TotalSeconds : 1;
            var totalTime = trackInfo.Duration.TotalSeconds;

            var currentPersent = estematedTime / totalTime;
            int currentPos = (int)(currentPersent * 10);
            var timeString = "----------";
            var bulder = new StringBuilder(timeString);
            bulder.Insert(currentPos, "|");

            var timeStart = "00:00";
            var timeEnd = trackInfo.Duration.ToString(@"mm\:ss");

            var timelineString = $"{timeStart} {bulder} {timeEnd}";

            return timelineString;
        }

        public static MusicTrackInfo AsMusicTrackInfo(this LavalinkTrack lavaTrack)
        {
            if (lavaTrack == null) return null;
            return new MusicTrackInfo()
            {
                Author = lavaTrack.Author,
                Title = lavaTrack.Title,
                Url = lavaTrack.Source,
                Duration = lavaTrack.Duration,
                Position = lavaTrack.Position
            };
        }
    }
}
