using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class Connections
    {
        [JsonPropertyName("group-affiliation")]
        public string GroupAffiliation { get; set; }
        [JsonPropertyName("relatives")]
        public string Relatives { get; set; }
    }
}
