using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class HeroInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("powerstats")]
        public Powerstats Powerstats { get; set; }
        [JsonPropertyName("biography")]
        public Biography Biography { get; set; }
        [JsonPropertyName("appearance")]
        public Appearance Appearance { get; set; }
        [JsonPropertyName("work")]
        public Work Work { get; set; }
        [JsonPropertyName("connections")]
        public Connections Connections { get; set; }
        [JsonPropertyName("image")]
        public Image Image { get; set; }
    }
}
