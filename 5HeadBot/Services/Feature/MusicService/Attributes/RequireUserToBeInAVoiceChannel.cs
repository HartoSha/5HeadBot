using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.MusicService.Attributes
{
    public sealed class RequireUserToBeInAVoiceChannel : PreconditionAttribute
    {
        public override string ErrorMessage { get; set; }
        public override async Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context, CommandInfo command, IServiceProvider services
        )
        {
            var channel = (context.User as IVoiceState)?.VoiceChannel;
            if (channel is null)
            {
                return await Task.FromResult(PreconditionResult.FromError(
                    !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage :
                    "You should be in a voice channel"
                ));
            }

            return PreconditionResult.FromSuccess();
        }
    }
}
