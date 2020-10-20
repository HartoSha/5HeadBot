using _5HeadBot.Services.BotMessageService;
using _5HeadBot.Services.BotMessageService.Data;
using Discord;
using Discord.Commands;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Internal
{
    public class MessageSenderModuleBase : ModuleBase<SocketCommandContext>
    {
        private BotMessageSender _sender;
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

        private BotMessageBuilder _builder;
        public BotMessageBuilder NewMessage { 
            get => _builder; 
            set 
            {
                if (value is BotMessageBuilder)
                    _builder = value;
                else
                    throw new ArgumentException($"Forbidden override of {nameof(BotMessageBuilder)}.");
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
