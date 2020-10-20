using _5HeadBot.Services.BotMessageService.Data;
using Discord;

namespace _5HeadBot.Services.BotMessageService
{
    public static class BotMessageContentStyler
    {
        public static BotMessageContent ApplyStyle(BotMessageContent content, BotMessageStyle style)
        {
            var styledText = content?.Text?.Length > 0 ? ApplyStyle(content.Text, style) : null;
            var styledEmbed = ApplyStyle(content?.Embed, style);

            return new BotMessageContent(styledText, styledEmbed);
        }

        private static string ApplyStyle(string text, BotMessageStyle displayType)
        {
            return displayType switch
            {
                BotMessageStyle.Warning => $"Warning: {text}",
                BotMessageStyle.Error => $"Error: {text}",
                BotMessageStyle.Exception => $"Exception: {text}",
                _ => text,
            };
        }
        private static Embed ApplyStyle(Embed embed, BotMessageStyle displayType)
        {
            return displayType switch
            {
                BotMessageStyle.Success =>
                    embed?.
                        ToEmbedBuilder().
                        WithColor(Color.Green).
                        Build(),

                BotMessageStyle.Warning => 
                    embed?.
                        ToEmbedBuilder().
                        WithColor(Color.Orange).
                        WithTitle($"Warning: {embed.Title}").
                        Build(),

                BotMessageStyle.Error => 
                    embed?.
                        ToEmbedBuilder().
                        WithColor(Color.Red).
                        WithTitle($"Error: {embed.Title}").
                        Build(),

                BotMessageStyle.Exception => 
                    embed?.
                        ToEmbedBuilder().
                        WithColor(Color.DarkRed).
                        WithTitle($"Exception: {embed.Title}").
                        Build(),

                _ => embed,
            };
        }
    }
}
