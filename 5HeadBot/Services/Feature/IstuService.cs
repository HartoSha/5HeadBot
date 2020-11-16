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
            if (_cache.GetValue("istu_week_status") is string cachedStatus)
                return cachedStatus;

            var resp = await _network.GetAsync(ISTU_URL, useBrowserEmulation: true);

            if (resp.IsSuccessStatusCode)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await resp.Content.ReadAsStringAsync());
                var node = doc.DocumentNode.
                    SelectSingleNode("//div[contains(@class, 'site-header-top-element ref-week type-separated')]");

                var weekStatus = node?.InnerText;

                // only cache if status was found
                if (weekStatus != null && !string.IsNullOrEmpty(weekStatus))
                {
                    // calculate time to the next monday
                    var toNextMonday = GetNextMonday() - DateTime.Now;
                    _cache.Add("istu_week_status", weekStatus, DateTimeOffset.Now.Add(toNextMonday));
                }

                return weekStatus ?? "Неделя не может быть определена.";
            }

            else return $"Error. {ISTU_URL} does not respond.";
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
