using _5HeadBot.Services.NetworkService;
using _5HeadBot.Services.PictureService.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace _5HeadBot.Services.PictureService.PictureProviders
{
    public class Catas : ICatImageProvider
    {
        private readonly NetWorker _worker;
        public Catas(NetWorker worker)
            => _worker = worker;
        public async Task<Stream> GetCatPictureAsync()
        {
            var resp = await _worker.GetAsync("https://cataas.com/cat");
            if(resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadAsStreamAsync();
            }
            return null;
        }
    }
}
