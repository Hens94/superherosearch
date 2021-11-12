using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;
using HGUtils.Common.Models;
using HGUtils.Helpers.HttpClient;
using SuperHeroSearch_App.Contracts.HttpClients;
using SuperHeroSearch_App.Interfaces;
using SuperHeroSearch_App.Models.HttpClients.Response;
using SuperHeroSearch_App.Utils;
using SuperHeroSearch_Common.Configurations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SuperHeroSearch_App.Services.HttpClients
{
    public class SuperHeroHttpClientService : ISuperHeroHttpClient
    {
        private readonly ApiConfig _apiConfig;
        private readonly HttpClient _httpClient;

        public SuperHeroHttpClientService(
            ApiConfig apiConfig,
            HttpClient httpClient)
        {
            _apiConfig = apiConfig;
            _httpClient = httpClient;
        }

        private async Task<IResult<TResult>> SendRequest<TResult>(
            HttpRequestMessage request) where TResult : class, IBaseResponse
        {
            var response = await _httpClient.SendAsync(
                httpRequest: request,
                writeInLogs: _apiConfig.WriteInLogs);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var content = JsonSerializer.Deserialize<TResult>(jsonContent);

                if (!content.Response.Equals("success"))
                {
                    return new Result<TResult>(message: content.Error ?? "Uncontrolled error", resultType: ResultType.ThirdPartyError);
                }

                return new Result<TResult>(content);
            }

            return new Result<TResult>(message: "SuperHero Api is not available", resultType: ResultType.ThirdPartyError);
        }

        public async Task<IResult<GetInfoResponse>> GetCharacter(string id)
        {
            using var request = new HttpRequestMessage(
                       HttpMethod.Get,
                       _apiConfig.Endpoints.Info.AssignRouteParameters((nameof(id), id)));

            return await SendRequest<GetInfoResponse>(request);
        }

        public async Task<IResult<SearchResponse>> Search(string name)
        {
            using var request = new HttpRequestMessage(
                       HttpMethod.Get,
                       _apiConfig.Endpoints.Search.AssignRouteParameters((nameof(name), name)));

            return await SendRequest<SearchResponse>(request);
        }
    }
}
