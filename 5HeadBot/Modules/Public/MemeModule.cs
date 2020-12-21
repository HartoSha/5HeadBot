using _5HeadBot.Modules.Internal;
using _5HeadBot.Services.Core.BotMessageService;
using _5HeadBot.Services.Core.BotMessageService.Data;
using _5HeadBot.Services.Feature.MemeService;
using Discord.Commands;
using System.Threading.Tasks;

namespace _5HeadBot.Modules.Public
{
    public class MemeModule : MessageSenderModuleBase
    {
        public MemeService _memes;
        public MemeModule(MemeService memes)
        {
            _memes = memes;
        }

        [Command("Хочу шутку")]
        [Alias("Шутка", "Шутку", "meme", "gimme")]
        public async Task GetMeme()
        {
            var meme = await _memes.GetMemeAsync();

            if(meme is null)
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle("No memes left for today =(")
                    .WithDisplayType(BotMessageStyle.Error)
                );
                return;
            }

            await ReplyAsync(
                new BotMessageBuilder()
                .WithEmbedWithTitle(meme.Title)
                .WithEmbedWithUrl(meme.SourceUrl)
                .WithEmbedWithImageUrl(meme.ContentUrl)
                .WithDisplayType(BotMessageStyle.Success)
            );
        }
    }
}
