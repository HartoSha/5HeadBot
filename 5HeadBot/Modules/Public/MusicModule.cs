using _5HeadBot.Modules.Internal;
using _5HeadBot.Services;
using _5HeadBot.Services.BotMessageService;
using _5HeadBot.Services.BotMessageService.Data;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    [Group("Music")]
    [Alias("Mus", "M")]
    [RequireContext(ContextType.Guild, ErrorMessage = "You can use music commads only in guild.")]
    [RequireBotPermission(GuildPermission.Connect, ErrorMessage = "Bot should have permission to connect to a voice channel.")]
    [RequireBotPermission(GuildPermission.Speak, ErrorMessage = "Bot should have permission to speak.")]
    public class MusicModule : MessageSenderModuleBase
    {
        public MusicService MusicService { get; set; }
        [Command("Join")]
        public async Task Join()
        {
            var channel = (Context.User as IVoiceState)?.VoiceChannel;
            if (channel is null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("You should be in a voice channel.")
                    .WithDisplayType(BotMessageStyle.Warning)
                );
                return;
            }

            await Leave();
            await MusicService.JoinAsync(channel);
        }

        [Command("Leave")]
        public async Task Leave()
        {
            var channel = (Context.User as IVoiceState)?.VoiceChannel;
            if (channel is null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("You should be in a voice channel.")
                    .WithDisplayType(BotMessageStyle.Warning)
                );
                return;
            }
            
            await MusicService.LeaveAsync(channel);
        }

        [Command("Play")]
        public async Task Play(params string[] query)
        {
            if (query.Length == 0)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle(
                        "A blank request is given.\n" +
                        "There is only the great void and nothing else.")
                    .WithEmbedColor(Color.DarkerGrey)
                );
                return;
            }
                
            await ReplyAsync(await MusicService.PlayAsync(string.Join(" ", query), Context.Guild));
        }

        [Command("Skip")]
        public async Task Skip()
        {
            await ReplyAsync(await MusicService.SkipAsync(Context.Guild));
        }
    }
}
