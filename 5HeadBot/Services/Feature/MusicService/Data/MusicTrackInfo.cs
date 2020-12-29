using System;

namespace _5HeadBot.Services.Feature.MusicService.Data
{
    public class MusicTrackInfo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Position { get; set; }
    }
}
