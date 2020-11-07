using _5HeadBot.Services.NetworkService;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace _5HeadBot.Services.MemeService
{
    public class MemeService
    {
        private readonly NetWorker _worker;
        public MemeService(NetWorker worker)
        {
            _worker = worker;
        }

        const string MEMES_PROVIDER_URL = "https://meme-api.herokuapp.com/gimme";
        public async Task<MemeServiceResponce> GetMemeAsync()
        {
            var result = new MemeServiceResponce();

            var responce = await _worker.GetDeserializedAsync<Meme>(MEMES_PROVIDER_URL);

            if(responce.Exception is null)
                result.Meme = responce.DesirializedContent;
            
            else
                result.ErrorMessage = "No memes left for today.";

            return result;
        }
    }
}
