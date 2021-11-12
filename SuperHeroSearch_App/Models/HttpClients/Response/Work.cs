using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class Work
    {
        [JsonPropertyName("occupation")]
        public string Occupation { get; set; }
        [JsonPropertyName("base")]
        public string Base { get; set; }
    }
}
