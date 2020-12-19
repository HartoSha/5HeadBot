using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.MusicService.Attributes
{
    public sealed class RequireBotToBeInAVoiceChannel : PreconditionAttribute
    {
        public override string ErrorMessage { get; set; }
        public async override Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context, CommandInfo command, IServiceProvider services
        )
        {
            var isBotInVoice = 
                (await context.Channel.GetUserAsync(context.Client.CurrentUser.Id) as IGuildUser)
                .VoiceChannel != null;

            if (isBotInVoice)
            {
                return PreconditionResult.FromSuccess();
            }

            return PreconditionResult.FromError(
                !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage :
                "Bot should be in a voice channel to be able to execute this command."
            );
        }
    }
}
