using _5HeadBot.DTOS;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class ConfigService
    {
        public ConfigDTO Config { get; private set; }

        /// <summary>
        /// Trys to find a correct config.json file going up to a root folder.
        /// Environment variables override settings of config.json file.
        /// </summary>
        public async Task InitializeAsync()
        {
            // initailaize config from config.json
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            do
            {
                var path = Path.Combine(currentDir?.FullName, "config.json");
                if (File.Exists(path))
                    Config = JsonConvert.DeserializeObject<ConfigDTO>(
                        await File.ReadAllTextAsync(path)
                    );
                currentDir = currentDir?.Parent;
            } while (currentDir != null);

            // override config from environment variables
            Config = new ConfigDTO(
                discordToken: Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? Config.DiscordToken,
                googleApiKey: Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ?? Config.GoogleApiKey,
                googleSearchEngineKey: Environment.GetEnvironmentVariable("GOOGLE_SEARCH_ENGINE_KEY") ?? Config.GoogleSearchEngineKey,
                catApiKey: Environment.GetEnvironmentVariable("CAT_API_KEY") ?? Config.CatApiKey
            );

            var errorMessage =
                (string.IsNullOrEmpty(Config.DiscordToken) ? "DiscordToken is not provided " : "") +
                (string.IsNullOrEmpty(Config.GoogleApiKey) ? "GoogleApiKey is not provided " : "") +
                (string.IsNullOrEmpty(Config.GoogleSearchEngineKey) ? "GoogleSearchEngineKey is not provided " : "") +
                (string.IsNullOrEmpty(Config.CatApiKey) ? "CatApiKey is not provided" : "");

            if (!string.IsNullOrEmpty(errorMessage))
                throw new ConfigurationException(errorMessage);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(Config);
        }
        private class ConfigurationException : Exception
        {
            public ConfigurationException(string message) : base(message) { }
        }
    }
}
