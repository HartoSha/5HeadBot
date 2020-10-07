using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class IstuService
    {
        private readonly HttpClient _client;
        public IstuService(HttpClient client)
        {
            this._client = client;
        }
        private readonly string ISTU_URL = @"https:\\istu.ru";
        public async Task<string> GetWeekStatus()
        {
            // Todo: Неделя возвращается не правильно. Из-за исполняемого на клиенте js кода по просчету под/над чертой
            var responce = await _client.GetAsync(ISTU_URL);
            if (responce.IsSuccessStatusCode)
            {
                var htmlString = await responce.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlString);
                var node = doc.DocumentNode.
                    SelectSingleNode("//div[contains(@class, 'site-header-top-element ref-week type-separated site-header-top-element-text')]");
                return node?.InnerText ?? "Неделя не может быть определена.";
            }
            else return $"Error. {ISTU_URL} responded with code: {responce.StatusCode}";
        }
    }
}
