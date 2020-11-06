using _5HeadBot.Modules.Internal;
using _5HeadBot.Services;
using _5HeadBot.Services.BotMessageService;
using _5HeadBot.Services.BotMessageService.Data;
using Discord.Commands;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    [Group("Random")]
    [Alias("")]
    public class RNGmodule : MessageSenderModuleBase
    {
        public RNGService RNG { get; set; }

        [Command("Random")]
        [Summary("Returns a sequence of true and false")]
        [Alias("Flip", "Coin", "Rng")]
        public async Task Flip(int sequenceLength = 1)
        {
            if(sequenceLength <= 600)
            {
                await ReplyAsync(await RNG.GetRandomSequence(sequenceLength));
                return;
            }
            await ReplyAsync(
                new BotMessageBuilder().
                WithEmbedWithTitle($"Discord message can fit only 600 moon symbols.").
                WithDisplayType(BotMessageStyle.Warning)
            );
        }
    }
}
