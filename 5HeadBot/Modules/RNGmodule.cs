using _5HeadBot.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Modules
{
    public class RNGmodule : ModuleBase<SocketCommandContext>
    {
        public RNGService RNG { get; set; }
        [Command("flip")]
        [Summary("Returns a sequence of true and false")]
        [Alias("coin", "rng")]
        public async Task Flip(int sequenceLength = 1)
            => await ReplyAsync(await RNG.GetRandomSequence(sequenceLength));
    }
}
