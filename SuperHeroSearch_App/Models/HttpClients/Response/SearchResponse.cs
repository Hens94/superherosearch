using SuperHeroSearch_App.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class SearchResponse : IBaseResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
        [JsonPropertyName("results-for")]
        public string ResultsFor { get; set; }
        [JsonPropertyName("results")]
        public IEnumerable<HeroInfo> Results { get; set; }
    }
}
