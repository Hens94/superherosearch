using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class Image
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
