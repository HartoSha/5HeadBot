using _5HeadBot.Services.PictureService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Services.PictureService.PictureProviders
{
    public class Catas : ICatImageProvider
    {
        private readonly HttpClient _http;
        public Catas(HttpClient http)
            => _http = http;
        public async Task<Stream> GetCatPictureAsync()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat");
            if(resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadAsStreamAsync();
            }
            return null;
        }
    }
}
