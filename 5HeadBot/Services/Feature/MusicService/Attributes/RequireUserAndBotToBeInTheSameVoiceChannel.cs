using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.MusicService.Attributes
{
    public sealed class RequireUserAndBotToBeInTheSameVoiceChannel : PreconditionAttribute
    {
        public override string ErrorMessage { get; set; }
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var botChannel =
                (await context.Channel.GetUserAsync(context.Client.CurrentUser.Id) as IGuildUser)
                .VoiceChannel;

            var userChannel = (context.User as IVoiceState)?.VoiceChannel;

            if(userChannel is null ||
               botChannel  is null ||
               userChannel.Id != botChannel.Id)
            {
                return PreconditionResult.FromError(
                    !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage :
                    "User and bot should be in the same voice channel."
                );
            }
            
            return PreconditionResult.FromSuccess();
        }
    }
}
