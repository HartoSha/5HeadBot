using _5HeadBot.Services.Core.NetworkService;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature.MemeService
{
    public class MemeService
    {
        private readonly NetWorker _worker;
        public MemeService(NetWorker worker)
        {
            _worker = worker;
        }

        const string MEMES_PROVIDER_URL = "https://meme-api.herokuapp.com/gimme";
        public async Task<Meme> GetMemeAsync()
        {
            var responce = await _worker.GetDeserializedAsync<Meme>(MEMES_PROVIDER_URL);

            // TODO: Add logging
            // if(responce.Exception != null)
            // {
            //     Logger.Log(responce.Exception);
            // }

            return responce.DesirializedContent;
        }
    }
}
