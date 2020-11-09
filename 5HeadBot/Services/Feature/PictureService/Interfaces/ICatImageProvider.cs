using System.IO;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.PictureService.Interfaces
{
    public interface ICatImageProvider
    {
        public Task<Stream> GetCatPictureAsync();
    }
}
