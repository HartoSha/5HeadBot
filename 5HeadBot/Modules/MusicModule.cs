using _5HeadBot.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Modules
{
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        public MusicService MusicService { get; set; }
        [Command("Join")]
        public async Task Join()
        {
            var channel = (Context.User as IVoiceState)?.VoiceChannel;
            if (channel is null)
            {
                await ReplyAsync("You should be in a voice channel.");
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
                await ReplyAsync("You should be in a voice channel.");
                return;
            }

            await MusicService.LeaveAsync(channel);
        }
        [Command("play")]
        public async Task Play([Remainder] string query)
        {
            await ReplyAsync(await MusicService.PlayAsync(query, Context.Guild));
        }
    }
}
