using Discord;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Core.LoggingService
{
    public interface ILoggingService
    {
        Task LogAsync(LogMessage log);
    }
}
