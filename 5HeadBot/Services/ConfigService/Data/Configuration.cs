using Newtonsoft.Json;

namespace _5HeadBot.Services.ConfigService
{
    public class Configuration
    {
        [JsonProperty("discord-token")]
        public string DiscordToken { get; }

        [JsonProperty("google-api-key")]
        public string GoogleApiKey { get; }

        [JsonProperty("google-search-engine-key")]
        public string GoogleSearchEngineKey { get; }

        [JsonProperty("thecatapi.com-api-key")]
        public string CatApiKey { get; }
        public Configuration(
            string discordToken,
            string googleApiKey,
            string googleSearchEngineKey,
            string catApiKey
        )
        {
            DiscordToken = discordToken;
            GoogleApiKey = googleApiKey;
            GoogleSearchEngineKey = googleSearchEngineKey;
            CatApiKey = catApiKey;
        }
    }
}
