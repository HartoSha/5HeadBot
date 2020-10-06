using _5HeadBot.Services.PictureService.Interfaces;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace _5HeadBot.Services.PictureService
{
    public class PictureService
    {
        private readonly ICatImageProvider _catProvider;
        public PictureService(ICatImageProvider catProvider)
        {
            _catProvider = catProvider;
        }
        public async Task<Stream> GetCatPictureAsync()
        {
            return await _catProvider.GetCatPictureAsync();
        }
    }
}
