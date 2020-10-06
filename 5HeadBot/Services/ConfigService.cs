using _5HeadBot.DTOS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class ConfigService
    {
        public ConfigDTO Config { get; private set; }
        public ConfigService() { }
        public async Task InitializeAsync(string configJsonPath)
        {
            if (File.Exists(configJsonPath))
            {
                Config = JsonConvert.DeserializeObject<DTOS.ConfigDTO>(
                    await File.ReadAllTextAsync(configJsonPath)
                );
            }

        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(Config);
        }
        public class RequiredConfigNotFoundOrNullException : Exception
        {
            public RequiredConfigNotFoundOrNullException(string message) : base(message)
            {

            }

            public RequiredConfigNotFoundOrNullException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
}
