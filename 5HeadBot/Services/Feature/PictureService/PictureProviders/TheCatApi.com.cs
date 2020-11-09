using _5HeadBot.Services.Core.ConfigService;
using _5HeadBot.Services.Core.NetworkService;
using _5HeadBot.Services.Feature.PictureService.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.PictureService.PictureProviders
{
    public class TheCatApi : ICatImageProvider
    {
        private readonly NetWorker _worker;
        private readonly ConfigurationService _config;
        public TheCatApi(NetWorker worker, ConfigurationService config)
        {
            _worker = worker;
            _config = config;
        }

        public async Task<Stream> GetCatPictureAsync()
        {
            var queryEndpoint = $"https://api.thecatapi.com/v1/images/search/?api_key={_config.Config.CatApiKey}";

            #region Expected json responce type
            /*
                Expected json responce type:
                [
                {
                    "breeds": [],
                    "id": "avh",
                    "url": "https://cdn2.thecatapi.com/images/avh.jpg",
                    "width": 500,
                    "height": 313
                }
                ]
            */
            #endregion

            var type = new[]
            {
                new
                {
                    url = "",
                }
            };

            // get responce from an api, then go to picture page and get it

            var apiResponce = await _worker.GetDeserializedAsync(queryEndpoint, type);


            Stream result = null;
            if(apiResponce.HttpResponseMessage.IsSuccessStatusCode)
            {
                var imgStream = await(
                    await _worker.GetAsync(apiResponce.DesirializedContent.FirstOrDefault().url)
                ).Content.ReadAsStreamAsync();

                result = imgStream;
            }

            return result;
        }
    }
}
