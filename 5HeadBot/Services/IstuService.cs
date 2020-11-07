using _5HeadBot.Services.NetworkService;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class IstuService
    {
        private readonly NetWorker _network;
        public IstuService(NetWorker network)
        {
            this._network = network;
        }
        private readonly string ISTU_URL = @"https://istu.ru";
        public async Task<string> GetWeekStatus()
        {

            var resp = await _network.GetAsync(ISTU_URL, useBrowserEmulation: true);

            if (resp.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await resp.Content.ReadAsStringAsync());
                var node = doc.DocumentNode.
                    SelectSingleNode("//div[contains(@class, 'site-header-top-element ref-week type-separated')]");
                return node?.InnerText ?? "Неделя не может быть определена.";
            }

            else return $"Error. {ISTU_URL} does not respond.";
        }
    }
}
