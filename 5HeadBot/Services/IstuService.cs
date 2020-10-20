using HtmlAgilityPack;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class IstuService
    {
        private readonly BrowserService _browser;
        public IstuService(BrowserService browser)
        {
            this._browser = browser;
        }
        private readonly string ISTU_URL = @"https:\\istu.ru";
        public async Task<string> GetWeekStatus()
        {
            if (!_browser.Connected) 
            {
                throw new System.Exception("Browser is not connnected");
            } 

            if (await _browser.UrlIsResponding(ISTU_URL))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await _browser.DownloadAsync(ISTU_URL));
                var node = doc.DocumentNode.
                    SelectSingleNode("//div[contains(@class, 'site-header-top-element ref-week type-separated')]");
                return node?.InnerText ?? "Неделя не может быть определена.";
            }
            else return $"Error. {ISTU_URL} does not respond.";
        }
    }
}
