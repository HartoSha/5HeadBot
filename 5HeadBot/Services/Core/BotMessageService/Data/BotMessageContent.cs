using Discord;
namespace _5HeadBot.Services.Core.BotMessageService.Data
{
    public class BotMessageContent
    {
        public string Text { get; }
        public Embed Embed { get; }
        public BotMessageContent(string text, Embed embed)
        {
            Text = text;
            Embed = embed;
        }
    }
}
