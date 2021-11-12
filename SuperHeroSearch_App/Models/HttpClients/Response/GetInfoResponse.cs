using SuperHeroSearch_App.Interfaces;
using System.Text.Json.Serialization;

namespace SuperHeroSearch_App.Models.HttpClients.Response
{
    public class GetInfoResponse : HeroInfo, IBaseResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
