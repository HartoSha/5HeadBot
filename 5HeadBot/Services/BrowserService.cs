using HtmlAgilityPack;
using PuppeteerSharp;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class BrowserService
    {
        private Browser _browser;
        private Page _page;
        private HttpClient _httpClient;
        public bool Connected { get => _browser != null && _browser.IsConnected; }
        public BrowserService()
        {
            _httpClient = new HttpClient();
        }
        public async Task InitializeAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });

            _page = await _browser.NewPageAsync();
            await _page.SetViewportAsync(new ViewPortOptions() { Width = 1920, Height = 1080 });
        }

        public async Task<string> DownloadAsync(string url)
        {
            await _page?.GoToAsync(url);
            return await _page?.GetContentAsync();
        }

        public async Task<bool> UrlIsResponding(string url)
        {
            var responce = await _httpClient.GetAsync(url);
            return responce.IsSuccessStatusCode;
        }
    }
}
