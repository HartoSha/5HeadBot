using Newtonsoft.Json;

namespace _5HeadBot.Services.MemeService
{
    public class Meme
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string ContentUrl { get; }

        [JsonProperty("postLink")]
        public string SourceUrl { get; }
        public Meme(string sourceUrl, string contentUrl)
        {
            ContentUrl = contentUrl;
            SourceUrl = sourceUrl;
        }
    }
}
