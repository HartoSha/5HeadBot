namespace _5HeadBot.Services.BotMessageService.Data
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
