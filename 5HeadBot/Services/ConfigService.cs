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
        public ConfigService() { }

        /// <summary>
        /// Trys to find a correct config file <paramref name="configJsonPath"/> going up <paramref name="searchDepth"/> times
        /// </summary>
        /// <param name="configJsonPath">JSON configuration file/path to find</param>
        /// <param name="searchDepth">how many times try to go up a folder</param>
        /// <returns></returns>

        public async Task InitializeAsync(string configJsonPath, int searchDepth = 3)
        {
            string toBaseDir = "";
            for (int i = 0; i < searchDepth; i++)
            {
                var path = Path.Combine(toBaseDir, configJsonPath);
                if (File.Exists(path))
                {
                    try {
                        Config = JsonConvert.DeserializeObject<DTOS.ConfigDTO>(
                            await File.ReadAllTextAsync(path)
                        );
                    } catch { }
                }
                toBaseDir += "../";
            }
            if(Config is null)
            {
                throw new RequiredConfigNotFoundOrNullException(@$"Configuration file not found or not correct '{configJsonPath}'");
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(Config);
        }
        public class RequiredConfigNotFoundOrNullException : Exception
        {
            public RequiredConfigNotFoundOrNullException(string message) : base(message) { }
            public RequiredConfigNotFoundOrNullException(string message, Exception innerException) : base(message, innerException){ }
        }
    }
}
