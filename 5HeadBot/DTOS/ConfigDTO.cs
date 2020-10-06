using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _5HeadBot.DTOS
{
    public class ConfigDTO
    {
        [JsonProperty("discord-token")]
        public string DiscordToken { get; }

        [JsonProperty("google-api-key")]
        public string GoogleApiKey { get; }

        [JsonProperty("google-search-engine-key")]
        public string GoogleSearchEngineKey { get; }

        [JsonProperty("thecatapi.com-api-key")]
        public string CatApiKey { get; }
        public ConfigDTO(
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
