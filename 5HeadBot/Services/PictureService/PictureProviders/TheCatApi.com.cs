using _5HeadBot.Services.PictureService.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Services.PictureService.PictureProviders
{
    public class TheCatApi : ICatImageProvider
    {
        private readonly HttpClient _http;
        private readonly ConfigService _config;
        public TheCatApi(HttpClient http, ConfigService config)
        {
            _http = http;
            _config = config;
        }

        public async Task<Stream> GetCatPictureAsync()
        {
            var queryEndpoint = $"https://api.thecatapi.com/v1/images/search/?api_key={_config.Config.CatApiKey}";
            var resp = await _http.GetAsync(queryEndpoint);
            
            if(resp.IsSuccessStatusCode)
            {
                string json = await resp.Content.ReadAsStringAsync();

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
                var jsonObject = JsonConvert.DeserializeAnonymousType(json,
                    new[] 
                    {
                        new 
                        {
                            url = "",
                        }
                    }
                );

                if(jsonObject != null)
                {
                    string catImgUrl = jsonObject.FirstOrDefault().url;
                    var catImg = await _http.GetAsync(catImgUrl);
                    return await catImg.Content.ReadAsStreamAsync();
                }
            }
            return null;
        }
    }
}
