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
            var memeResp = await _memes.GetMemeAsync();

            if (memeResp.ErrorMessage != null)
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle(memeResp.ErrorMessage)
                    .WithDisplayType(BotMessageStyle.Error)
                );

            else
            {
                await ReplyAsync(
                    new BotMessageBuilder()
                    .WithEmbedWithTitle(memeResp.Meme.Title)
                    .WithEmbedWithUrl(memeResp.Meme.SourceUrl)
                    .WithEmbedWithImageUrl(memeResp.Meme.ContentUrl)
                    .WithDisplayType(BotMessageStyle.Success)
                );
            }
        }
    }
}
