using _5HeadBot.Services.Core.NetworkService.Deserializers;
using PuppeteerSharp;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Core.NetworkService
{
    /// <summary>
    /// This service makes http requests using browser emulation or a simple http client.
    /// It also desirializes json responses.
    /// It should be used when possible for all web reqests.
    /// </summary>
    public class NetWorker : IDisposable, IAsyncDisposable
    {
        private Browser _browser;
        private Browser ConnectedBrowser 
            => _browser.IsConnected ? _browser : throw new Exception("Browser is not connected");

        private Page _page;
        private Page AvaliablePage
            => !_page.IsClosed && _browser.IsConnected ? _page : throw new Exception("Browser page is not avaliable");

        private HttpClient _httpClient = new HttpClient();

        private IDeserializer _defaultDeserializer;

        public NetWorker(IDeserializer defaultDeserializer)
        {
            _defaultDeserializer = defaultDeserializer;
        }
        public async Task InitializeAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions() { Width = 1920, Height = 1080 },
                Args = new string[] { "--no-sandbox", "--disable-setuid-sandbox" }
            });

            _page = (await ConnectedBrowser.PagesAsync()).FirstOrDefault();
        }
        /// <summary>
        /// Gets <see cref="HttpResponseMessage"/> from an url by chosen way
        /// </summary>
        public async Task<HttpResponseMessage> GetAsync(string url, bool useBrowserEmulation = false)
        {
            HttpResponseMessage responce;
            if (useBrowserEmulation)
            {
                // go to the page
                // wait to .js executing and page loading...
                var response = await AvaliablePage.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);

                // then read contents of the page
                var content = new StringContent(await _page.GetContentAsync());
                responce = new HttpResponseMessage(response.Status)
                {
                    Content = content
                };
            }
            else
            {
                responce = await _httpClient.GetAsync(url);
            }
            return responce;
        }

        /// <summary>
        /// Makes a http GET request on a given <paramref name="url"/>, desirializes responce to an obect of type <typeparamref name="T"/> 
        /// </summary>
        /// <typeparam name="T">Type, response is desirialized to</typeparam>
        /// <param name="url">Request url</param>
        /// <param name="deserializer"> Desirializer is used to desirialize responce to <typeparamref name="T"/>. If <paramref name="deserializer"/> is null, then the default deserializer is taken from constructor's argument</param>
        /// <param name="useBrowserEmulation">Use browser emulation (executes .js, heavy) or a simple http client (gets only html, lightweight)</param>
        /// <returns><see cref="GenericResponseMessage{T}"/> containing default <see cref="HttpResponseMessage"/> and deserialized object <typeparamref name="T"/>. Check <see cref="GenericResponseMessage{T}.Exception"/> for deserialization exceptions</returns>
        public async Task<GenericResponseMessage<T>> GetDeserializedAsync<T>(string url, IDeserializer deserializer = null, bool useBrowserEmulation = false)
        {
            var responce = await this.GetAsync(url, useBrowserEmulation);

            var deserializerToBeUsed = deserializer ?? _defaultDeserializer;
            // desirialize responce string
            // create responce message
            Exception exception = null;
            T resultContent = default;
            try
            {
                resultContent = deserializerToBeUsed.DeserializeObject<T>(await responce.Content.ReadAsStringAsync());
            }
            catch(Exception ex)
            {
                exception = ex;
            }
            
            return new GenericResponseMessage<T>(responce, resultContent, exception);
        }
        /// <summary>
        /// Makes a http GET request on a given <paramref name="url"/>, desirializes responce to an <paramref name="anonymousTypeObject"/>
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="anonymousTypeObject"/></typeparam>
        /// <param name="url">Request url</param>
        /// <param name="anonymousTypeObject">Anonymous type object</param>
        /// <param name="deserializer">Desirializer is used to desirialize responce to <typeparamref name="T"/>. If <paramref name="deserializer"/> is null, then the default deserializer is taken from constructor's argument</param>
        /// <param name="useBrowserEmulation">Use browser emulation (executes .js, heavy) or a simple http client (gets only html, lightweight)</param>
        /// <returns><see cref="GenericResponseMessage{T}"/> containing default <see cref="HttpResponseMessage"/> and deserialized object <typeparamref name="T"/>. Check <see cref="GenericResponseMessage{T}.Exception"/> for deserialization exceptions</returns>
        public async Task<GenericResponseMessage<T>> GetDeserializedAsync<T>(string url, T anonymousTypeObject, IDeserializer deserializer = null, bool useBrowserEmulation = false)
        {
            var responce = await this.GetAsync(url, useBrowserEmulation);

            var deserializerToBeUsed = deserializer ?? _defaultDeserializer;
            // desirialize responce string
            // create responce message
            Exception exception = null;
            T resultContent = default;
            try
            {
                resultContent = deserializerToBeUsed.DeserializeAnonymousType(await responce.Content.ReadAsStringAsync(), anonymousTypeObject);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return new GenericResponseMessage<T>(responce, resultContent, exception);
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
