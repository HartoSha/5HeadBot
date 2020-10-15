using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class SearchService
    {
        private readonly HttpClient _http;
        private readonly ConfigService _config;
        public SearchService(HttpClient http, ConfigService config)
        {
            _http = http;
            _config = config;
        }

        public async Task<SearchResult> SearchAsync(string query)
        {
            string queryEndpoint = 
                $"https://www.googleapis.com/customsearch/v1?" +
                $"key={_config.Config.GoogleApiKey}" +
                $"&cx={_config.Config.GoogleSearchEngineKey}" +
                $"&q={query}";

            string json = await _http.GetAsync(queryEndpoint)?.
                Result?.Content?.ReadAsStringAsync();

            SearchResult res = JsonConvert.DeserializeObject<SearchResult>(json);
            return res;
        }
        public class SearchResult
        {
            public SearchError Error { get; set; }
            public List<SearchItem> Items { get; set; }
            public class SearchItem 
            {
                public string Title { get; set; }

                [JsonProperty("pagemap")]
                public Preview Thumbnail { get; set; }
                public string HtmlTitle { get; set; }
                public string Link { get; set; }
                public string Snippet { get; set; }
                public string HtmlSnippet { get; set; }
                public class Preview
                {
                    // my delicious pasta
                    // two keys (cse_thumbnail, cse_imgae) for one Images field
                    [JsonProperty("cse_thumbnail")]
                    public List<Image> Images { get; set; }
                    
                    [JsonProperty("cse_imgae")]
                    public List<Image> SetImages { set { if (Images.Count < 0) this.Images = value; } }

                    public class Image
                    {
                        [JsonProperty("src")]
                        public string Src { get; set; }
                    }
                }
            }
            public class SearchError
            {
                public string Code { get; set; }
                public string Message { get; set; }
                public override string ToString()
                {
                    return $"Error: {Code}. {Message}";
                }
            }
        }
    }
}