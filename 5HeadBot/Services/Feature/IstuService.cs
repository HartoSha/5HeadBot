using _5HeadBot.Services.Core.CachingService;
using _5HeadBot.Services.Core.NetworkService;
using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature
{
    public class IstuService
    {
        private readonly NetWorker _network;
        private readonly ICachingService _cache;

        public IstuService(NetWorker network, ICachingService cache)
        {
            this._network = network;
            this._cache = cache;
        }
        private readonly string ISTU_URL = @"https://istu.ru";
        public async Task<string> GetWeekStatus()
        {
            if (_cache.Get("istu_week_status") is string cachedStatus)
                return cachedStatus;

            var weekStatus = await TryGetIstuWeekStatusFromIstuWithBrowserEmulation(ISTU_URL);

            if (weekStatus != null && !string.IsNullOrEmpty(weekStatus))
            {
                _ = CacheUntillNextMonday("istu_week_status", weekStatus);
                return weekStatus;
            }

            return "Неделя не может быть определена";
        }

        private async Task<string> TryGetIstuWeekStatusFromIstuWithBrowserEmulation(string istuUrl)
        {
            var resp = await _network.GetAsync(istuUrl, useBrowserEmulation: true);
            if (resp.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await resp.Content.ReadAsStringAsync());
                var weekHtmlNode = doc.DocumentNode.
                    SelectSingleNode("//div[contains(@class, 'site-header-top-element ref-week type-separated')]");
                
                var istuWeekStatus = weekHtmlNode?.InnerText;
                return istuWeekStatus;
            }

            return null;
        }

        private DateTimeOffset CacheUntillNextMonday(string key, string value)
        {
            // calculate time to the next monday
            var nextMonday = GetNextMonday();
            var toNextMonday = nextMonday - DateTime.Today;

            // set expiration as next monday
            var absExpiration = new DateTimeOffset(DateTime.Today)
                .Add(toNextMonday);

            _cache.Set(key, value, absExpiration);

            return absExpiration;
        }

        private DateTime GetNextMonday()
        {
            // calculate date to the next monday
            DateTime today = DateTime.Today;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextMonday = today.AddDays(daysUntilMonday == 0 ? 7 : daysUntilMonday);
            return nextMonday;
        }
    }
}
