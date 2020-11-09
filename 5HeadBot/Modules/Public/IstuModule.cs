using _5HeadBot.Services.Feature;
using Discord.Commands;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    [Group("Istu")]
    [Alias("")]
    public class IstuModule : ModuleBase<SocketCommandContext>
    {
        public IstuService Istu { get; set; }

        [Command("неделя", RunMode = RunMode.Async)]
        [Alias("черта")]
        [Summary("Istu current week status")]
        public async Task WhatIsIstuCurrentWeek()
            => await ReplyAsync(await Istu.GetWeekStatus());
    }
}
