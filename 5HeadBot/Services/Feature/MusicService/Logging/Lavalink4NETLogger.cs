using _5HeadBot.Services.Core.LoggingService;
using Discord;
using Lavalink4NET.Logging;
using System;

namespace _5HeadBot.Services.Feature.MusicService.Logging
{
    class Lavalink4NETLogger : ILogger
    {
        private readonly ILoggingService logger;
        public Lavalink4NETLogger(ILoggingService logger)
        {
            this.logger = logger;
        }
        public void Log(object source, string message, LogLevel level = LogLevel.Information, Exception exception = null)
        {
            var servity = Lavalink4Net_LogLevel_To_DiscordNet_LogServity(level);
            logger.LogAsync(new LogMessage(servity, source.GetType().Name, message, exception));
        }

        private LogSeverity Lavalink4Net_LogLevel_To_DiscordNet_LogServity(LogLevel level) 
        {
            return level switch
            {
                LogLevel.Information => LogSeverity.Info,
                LogLevel.Error => LogSeverity.Error,
                LogLevel.Warning => LogSeverity.Warning,
                LogLevel.Debug => LogSeverity.Debug,
                LogLevel.Trace => LogSeverity.Debug,
                _ => throw new ArgumentOutOfRangeException(nameof(level), "Unexpected value"),
            };
        }
    }
}
