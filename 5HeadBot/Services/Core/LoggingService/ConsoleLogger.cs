using Discord;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Core.LoggingService
{
    public class ConsoleLogger : ILoggingService
    {
        public Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
