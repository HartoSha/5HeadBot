using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Core
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly BotMessageSender _messageSender;
        private readonly IServiceProvider _services;
        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _messageSender = services.GetRequiredService<BotMessageSender>();
            _services = services;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.  
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.

            // if it's not a dm and no prefix provided
            if (!(rawMessage.Channel is IPrivateChannel) 
                && !message.HasMentionPrefix(_discord.CurrentUser, ref argPos)
                // HasMentionPrefix doesn't accept messages with tags ended without a space char
                // so use of HasStringPrefix allows to accept those messages
                && !message.HasStringPrefix(_discord.CurrentUser.Mention, ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.

            SkipWhiteSpaces(ref argPos, message.Content);
            static void SkipWhiteSpaces(ref int argPos, string mess)
            {
                while(argPos < mess.Length 
                    && string.IsNullOrWhiteSpace(mess[argPos].ToString()))
                {
                    argPos++;
                }
            }

            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); let user know about this
            if (!command.IsSpecified)
            {
                await _messageSender.SendMessageAsync(
                    new BotMessageBuilder().
                        WithDisplayType(BotMessageStyle.Warning).
                        WithEmbedWithTitle($"Command not found. You can use `help` command to get a list of all avaliable commands.")
                        .Build(),
                    context
                );
                return;
            }    

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await _messageSender.SendMessageAsync(
                new BotMessageBuilder().
                    WithDisplayType(BotMessageStyle.Error).
                    WithEmbedWithTitle(result.ErrorReason)
                    .Build(),
                context
            );
        }
    }
}
