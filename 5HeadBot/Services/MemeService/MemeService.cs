using _5HeadBot.Services.BotMessageService.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace _5HeadBot.Services.MemeService
{
    public class MemeService
    {
        private readonly BrowserService _browser;
        public MemeService(BrowserService browser)
        {
            _browser = browser;
        }

        const string MEMES_PROVIDER_URL = "https://meme-api.herokuapp.com/gimme";
        public async Task<MemeServiceResponce> GetMemeAsync()
        {
            var resp = new MemeServiceResponce();
            if (await _browser.UrlIsRespondingAsync(MEMES_PROVIDER_URL))
            {
                var memeStr = await _browser.DownloadAsync(MEMES_PROVIDER_URL, false);
                resp.Meme = JsonConvert.DeserializeObject<Meme>(memeStr);
            }
            else resp.ErrorMessage = "No memes left for today.";
            return resp;
        }
    }
}
