using _5HeadBot.Services.Core.ConfigService;
using _5HeadBot.Services.Core.NetworkService;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _5HeadBot.Services.Feature
{
    public class SearchService
    {
        private readonly NetWorker _worker;
        private readonly ConfigurationService _config;
        public SearchService(NetWorker worker, ConfigurationService config)
        {
            _worker = worker;
            _config = config;
        }
        public async Task<SearchResult> SearchAsync(string query)
        {
            string queryEndpoint = 
                $"https://www.googleapis.com/customsearch/v1?" +
                $"key={_config.Config.GoogleApiKey}" +
                $"&cx={_config.Config.GoogleSearchEngineKey}" +
                $"&q={query}";

            return (await _worker.GetDeserializedAsync<SearchResult>(queryEndpoint)).DesirializedContent;
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