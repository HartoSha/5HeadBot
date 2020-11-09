namespace _5HeadBot.Services.Core.BotMessageService.Data
{
    public class BotMessage
    {
        public BotMessageContent Content { get; }
        public BotMessageStyle Style { get; }
        public BotMessage(BotMessageContent content, BotMessageStyle style = BotMessageStyle.Default)
        {
            Content = BotMessageContentStyler.ApplyStyle(content, style);
            Style = style;
        }
    }
}
