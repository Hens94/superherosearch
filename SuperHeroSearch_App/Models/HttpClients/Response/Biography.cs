using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class Biography
    {
        [JsonPropertyName("full-name")]
        public string Fullname { get; set; }
        [JsonPropertyName("alter-egos")]
        public string Alteregos { get; set; }
        [JsonPropertyName("aliases")]
        public IEnumerable<string> Aliases { get; set; }
        [JsonPropertyName("place-of-birth")]
        public string PlaceOfBirth { get; set; }
        [JsonPropertyName("first-appearance")]
        public string FirstAppearance { get; set; }
        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }
        [JsonPropertyName("alignment")]
        public string Alignment { get; set; }
    }
}
