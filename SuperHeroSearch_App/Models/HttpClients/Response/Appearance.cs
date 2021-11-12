using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class Appearance
    {
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
        [JsonPropertyName("race")]
        public string Race { get; set; }
        [JsonPropertyName("height")]
        public IEnumerable<string> Height { get; set; }
        [JsonPropertyName("weight")]
        public IEnumerable<string> Weight { get; set; }
        [JsonPropertyName("eye-color")]
        public string EyeColor { get; set; }
        [JsonPropertyName("hair-color")]
        public string HairColor { get; set; }
    }
}
