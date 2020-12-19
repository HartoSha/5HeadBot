using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using Discord;
using Lavalink4NET.Player;

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
                .WithEmbedWithFooter(new EmbedFooterBuilder().WithText(trackInfo.Author))
                .WithEmbedWithTitle(trackInfo.Title)
                .WithEmbedWithUrl(trackInfo.Url);
        }

        public static MusicTrackInfo AsMusicTrackInfo(this LavalinkTrack lavaTrack)
        {
            if (lavaTrack == null) return null;
            return new MusicTrackInfo()
            {
                Author = lavaTrack.Author,
                Title = lavaTrack.Title,
                Url = lavaTrack.Source
            };
        }
    }
}
