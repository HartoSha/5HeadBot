using _5HeadBot.Services.BotMessageService.Data;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace _5HeadBot.Services.BotMessageService
{
    public class BotMessageSender
    {
        public async Task SendMessageAsync(
            BotMessage message,
            ICommandContext context,
            bool isTTS = false,
            RequestOptions requestOptions = null
        )
        {
            if (message is null || context is null)
                return;

            var text = message.Content?.Text;
            var embed = message.Content?.Embed;

            if (string.IsNullOrEmpty(text) && embed is null)
                return;

            await context?.Channel?.SendMessageAsync(text, isTTS, embed, requestOptions);
        }
    }
}
