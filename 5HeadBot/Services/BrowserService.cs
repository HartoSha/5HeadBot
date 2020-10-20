using PuppeteerSharp;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    
    public class BrowserService : IDisposable, IAsyncDisposable
    {
        private Browser _browser;
        private Page _page;
        private HttpClient _httpClient = new HttpClient();
        public bool Connected { get => _browser != null && _browser.IsConnected; }
        public async Task InitializeAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });

            _page = (await _browser.PagesAsync()).FirstOrDefault();
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

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _httpClient?.Dispose();
                _page?.Dispose();
                _browser?.Dispose();
            }

            _httpClient = null;
            _page = null;
            _browser = null;
        }

        // Dispose async patten 
        // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_httpClient != null)
                await Task.Run(() => _httpClient.Dispose());

            if (_page != null)
                await _page.DisposeAsync();

            if(_browser != null)
                await _browser.DisposeAsync();

            _httpClient = null;
            _page = null;
            _browser = null;
        }
        #endregion
    }
}
