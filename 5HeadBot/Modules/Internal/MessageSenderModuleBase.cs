using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Internal
{
    public class MessageSenderModuleBase : ModuleBase<SocketCommandContext>
    {
        private BotMessageSender _sender;

        /// <summary>
        /// This property with exception in setter is used only in order 
        /// to inject dependency without using the class's constructor. 
        /// </summary>
        public BotMessageSender Sender
        {
            get => _sender;
            set
            {
                if (value is BotMessageSender)
                    _sender = value;
                else
                    throw new ArgumentException($"Forbidden override of {nameof(BotMessageSender)}.");
            }
        }
        public async Task ReplyAsync(BotMessage message = null, bool isTTS = false, RequestOptions options = null)
        {
            await _sender.SendMessageAsync(message, Context, isTTS, options);
        }
        public async Task ReplyAsync(BotMessageBuilder messageBuilder = null, bool isTTS = false, RequestOptions options = null)
        {
            await _sender.SendMessageAsync(messageBuilder?.Build(), Context, isTTS, options);
        }
    }
}
