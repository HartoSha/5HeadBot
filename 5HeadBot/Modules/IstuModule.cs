using _5HeadBot.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Modules
{
    public class IstuModule : ModuleBase<SocketCommandContext>
    {
        public IstuService Istu { get; set; }

        [Command("неделя")]
        [Alias("черта")]
        [Summary("Istu current week status")]
        public async Task WhatIsIstuCurrentWeek() 
            => await ReplyAsync(await Istu.GetWeekStatus());
    }
}
