using FluentAssertions;
using Flurl;
using HGUtils.Common.Enums;
using HGUtils.Common.Models;
using RichardSzalay.MockHttp;
using SuperHeroSearch_App.Contracts.HttpClients;
using SuperHeroSearch_App.Models.HttpClients.Response;
using SuperHeroSearch_App.Services.HttpClients;
using SuperHeroSearch_Common.Configurations;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SuperHeroSearch_Test.Services
{
    public class SuperHeroHttpClientServiceTests
    {
        private readonly ApiConfig _apiConfig = new ApiConfig
        {
            BaseUrl = "https://superheroapi.com/api",
            AccessToken = "10218877706629193",
            Endpoints = new Endpoints
            {
                Search = "search/{name}",
                Info = "{id}"
            },
            WriteInLogs = false
        };
        private readonly string _responseFormat = "application/json";

        [Fact]
        public async Task Search_ShouldReturnSuccess()
        {
            #region Arrange

            var searchName = "superman";
            var responseMessage = "success";

            var requestUrl = $"{_apiConfig.BaseUrl}/{_apiConfig.AccessToken}/search/{searchName}";

            var httpMock = new MockHttpMessageHandler();

            httpMock
                .When(requestUrl)
                .Respond(
                    HttpStatusCode.OK,
                    _responseFormat,
                    "{\"response\":\"success\",\"results-for\":\"superman\",\"results\":[{\"id\":\"195\",\"name\":\"Cyborg Superman\",\"powerstats\":{\"intelligence\":\"75\",\"strength\":\"93\",\"speed\":\"92\",\"durability\":\"100\",\"power\":\"100\",\"combat\":\"80\"},\"biography\":{\"full-name\":\"Henry Henshaw\",\"alter-egos\":\"No alter egos found.\",\"aliases\":[\"Grandmaster of the Manhunters\",\"Herald of the Anti-Monitor\",\"Alpha-Prime of the Alpha Lanterns\"],\"place-of-birth\":\"-\",\"first-appearance\":\"Adventures of Superman #466 (May, 1990)\",\"publisher\":\"DC Comics\",\"alignment\":\"bad\"},\"appearance\":{\"gender\":\"Male\",\"race\":\"Cyborg\",\"height\":[\"-\",\"0 cm\"],\"weight\":[\"- lb\",\"0 kg\"],\"eye-color\":\"Blue\",\"hair-color\":\"Black\"},\"work\":{\"occupation\":\"-\",\"base\":\"Warworld, Qward, Antimatter Universe, formerly Biot, Sector 3601\"},\"connections\":{\"group-affiliation\":\"Alpha Lantern Corps, Manhunters, Warworld, formerly Apokolips and Sinestro Corps\",\"relatives\":\"Terri Henshaw (wife, deceased)\"},\"image\":{\"url\":\"https://www.superherodb.com/pictures2/portraits/10/100/667.jpg\"}},{\"id\":\"644\",\"name\":\"Superman\",\"powerstats\":{\"intelligence\":\"94\",\"strength\":\"100\",\"speed\":\"100\",\"durability\":\"100\",\"power\":\"100\",\"combat\":\"85\"},\"biography\":{\"full-name\":\"Clark Kent\",\"alter-egos\":\"Superman Prime One-Million\",\"aliases\":[\"Clark Joseph Kent\",\"The Man of Steel\",\"the Man of Tomorrow\",\"the Last Son of Krypton\",\"Big Blue\",\"the Metropolis Marvel\",\"the Action Ace\"],\"place-of-birth\":\"Krypton\",\"first-appearance\":\"ACTION COMICS #1\",\"publisher\":\"Superman Prime One-Million\",\"alignment\":\"good\"},\"appearance\":{\"gender\":\"Male\",\"race\":\"Kryptonian\",\"height\":[\"6\",\"191 cm\"],\"weight\":[\"225 lb\",\"101 kg\"],\"eye-color\":\"Blue\",\"hair-color\":\"Black\"},\"work\":{\"occupation\":\"Reporter for the Daily Planet and novelist\",\"base\":\"Metropolis\"},\"connections\":{\"group-affiliation\":\"Justice League of America, The Legion of Super-Heroes (pre-Crisis as Superboy); Justice Society of America (pre-Crisis Earth-2 version); All-Star Squadron (pre-Crisis Earth-2 version)\",\"relatives\":\"Lois Lane (wife), Jor-El (father, deceased), Lara (mother, deceased), Jonathan Kent (adoptive father), Martha Kent (adoptive mother), Seyg-El (paternal grandfather, deceased), Zor-El (uncle, deceased), Alura (aunt, deceased), Supergirl (Kara Zor-El, cousin), Superboy (Kon-El/Conner Kent, partial clone)\"},\"image\":{\"url\":\"https://www.superherodb.com/pictures2/portraits/10/100/791.jpg\"}}]}");

            var httpClient = httpMock.ToHttpClient();
            httpClient.BaseAddress = new Uri(Url.Combine(_apiConfig.BaseUrl, _apiConfig.AccessToken, "/"));

            ISuperHeroHttpClient service = new SuperHeroHttpClientService(_apiConfig, httpClient);

            #endregion

            #region Act

            var response = await service.Search(searchName);

            #endregion

            #region Assert

            response.Should().NotBeNull();
            response.Should().BeOfType<Result<SearchResponse>>().Which.Code.Should().Be((int)ResultType.Success);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Results.Response.Should().Be(responseMessage);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Results.ResultsFor.Should().Be(searchName);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Results.Results.Should().NotBeEmpty();

            #endregion
        }

        [Fact]
        public async Task Search_ShouldReturnNotFound()
        {
            #region Arrange
            var searchName = "man not exists";
            var errorMessage = "character with given name not found";

            var requestUrl = $"{_apiConfig.BaseUrl}/{_apiConfig.AccessToken}/search/{searchName}";

            var httpMock = new MockHttpMessageHandler();

            httpMock
                .When(requestUrl)
                .Respond(
                    HttpStatusCode.OK,
                    _responseFormat,
                    "{\"response\":\"error\",\"error\":\"character with given name not found\"}");

            var httpClient = httpMock.ToHttpClient();
            httpClient.BaseAddress = new Uri(Url.Combine(_apiConfig.BaseUrl, _apiConfig.AccessToken, "/"));

            ISuperHeroHttpClient service = new SuperHeroHttpClientService(_apiConfig, httpClient);

            #endregion

            #region Act

            var response = await service.Search(searchName);

            #endregion

            #region Assert

            response.Should().NotBeNull();
            response.Should().BeOfType<Result<SearchResponse>>().Which.Code.Should().Be((int)ResultType.ThirdPartyError);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Message.Should().Be(errorMessage);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Results.Should().BeNull();

            #endregion
        }

        [Fact]
        public async Task Search_ShouldReturnUncontrolledError()
        {
            #region Arrange
            var searchName = "man not exists";
            var errorMessage = "SuperHero Api is not available";

            var requestUrl = $"{_apiConfig.BaseUrl}/{_apiConfig.AccessToken}/search/{searchName}";

            var httpMock = new MockHttpMessageHandler();

            httpMock
                .When(requestUrl)
                .Respond(
                    HttpStatusCode.InternalServerError,
                    _responseFormat,
                    "{}");

            var httpClient = httpMock.ToHttpClient();
            httpClient.BaseAddress = new Uri(Url.Combine(_apiConfig.BaseUrl, _apiConfig.AccessToken, "/"));

            ISuperHeroHttpClient service = new SuperHeroHttpClientService(_apiConfig, httpClient);

            #endregion

            #region Act

            var response = await service.Search(searchName);

            #endregion

            #region Assert

            response.Should().NotBeNull();
            response.Should().BeOfType<Result<SearchResponse>>().Which.Code.Should().Be((int)ResultType.ThirdPartyError);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Message.Should().Be(errorMessage);
            response.Should().BeOfType<Result<SearchResponse>>().Which.Results.Should().BeNull();

            #endregion
        }

        [Fact]
        public async Task Find_ShouldReturnSuccess()
        {
            #region Arrange

            var characterId = "1";
            var responseMessage = "success";

            var requestUrl = $"{_apiConfig.BaseUrl}/{_apiConfig.AccessToken}/{characterId}";

            var httpMock = new MockHttpMessageHandler();

            httpMock
                .When(requestUrl)
                .Respond(
                    HttpStatusCode.OK,
                    _responseFormat,
                    "{\"response\":\"success\",\"id\":\"1\",\"name\":\"A-Bomb\",\"powerstats\":{\"intelligence\":\"38\",\"strength\":\"100\",\"speed\":\"17\",\"durability\":\"80\",\"power\":\"24\",\"combat\":\"64\"},\"biography\":{\"full-name\":\"Richard Milhouse Jones\",\"alter-egos\":\"No alter egos found.\",\"aliases\":[\"Rick Jones\"],\"place-of-birth\":\"Scarsdale, Arizona\",\"first-appearance\":\"Hulk Vol 2 #2 (April, 2008) (as A-Bomb)\",\"publisher\":\"Marvel Comics\",\"alignment\":\"good\"},\"appearance\":{\"gender\":\"Male\",\"race\":\"Human\",\"height\":[\"6'8\",\"203 cm\"],\"weight\":[\"980 lb\",\"441 kg\"],\"eye-color\":\"Yellow\",\"hair-color\":\"No Hair\"},\"work\":{\"occupation\":\"Musician, adventurer, author; formerly talk show host\",\"base\":\"-\"},\"connections\":{\"group-affiliation\":\"Hulk Family; Excelsior (sponsor), Avengers (honorary member); formerly partner of the Hulk, Captain America and Captain Marvel; Teen Brigade; ally of Rom\",\"relatives\":\"Marlo Chandler-Jones (wife); Polly (aunt); Mrs. Chandler (mother-in-law); Keith Chandler, Ray Chandler, three unidentified others (brothers-in-law); unidentified father (deceased); Jackie Shorr (alleged mother; unconfirmed)\"},\"image\":{\"url\":\"https://www.superherodb.com/pictures2/portraits/10/100/10060.jpg\"}}");

            var httpClient = httpMock.ToHttpClient();
            httpClient.BaseAddress = new Uri(Url.Combine(_apiConfig.BaseUrl, _apiConfig.AccessToken, "/"));

            ISuperHeroHttpClient service = new SuperHeroHttpClientService(_apiConfig, httpClient);

            #endregion

            #region Act

            var response = await service.GetCharacter(characterId);

            #endregion

            #region Assert

            response.Should().NotBeNull();
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Code.Should().Be((int)ResultType.Success);
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Results.Response.Should().Be(responseMessage);
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Results.Id.Should().Be(characterId);

            #endregion
        }

        [Fact]
        public async Task Find_ShouldReturnNotFound()
        {
            #region Arrange
            var characterId = "999";
            var errorMessage = "invalid id";

            var requestUrl = $"{_apiConfig.BaseUrl}/{_apiConfig.AccessToken}/{characterId}";

            var httpMock = new MockHttpMessageHandler();

            httpMock
                .When(requestUrl)
                .Respond(
                    HttpStatusCode.OK,
                    _responseFormat,
                    "{\"response\":\"error\",\"error\":\"invalid id\"}");

            var httpClient = httpMock.ToHttpClient();
            httpClient.BaseAddress = new Uri(Url.Combine(_apiConfig.BaseUrl, _apiConfig.AccessToken, "/"));

            ISuperHeroHttpClient service = new SuperHeroHttpClientService(_apiConfig, httpClient);

            #endregion

            #region Act

            var response = await service.GetCharacter(characterId);

            #endregion

            #region Assert

            response.Should().NotBeNull();
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Code.Should().Be((int)ResultType.ThirdPartyError);
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Message.Should().Be(errorMessage);
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Results.Should().BeNull();

            #endregion
        }

        [Fact]
        public async Task Find_ShouldReturnUncontrolledError()
        {
            #region Arrange
            var characterId = "999";
            var errorMessage = "SuperHero Api is not available";

            var requestUrl = $"{_apiConfig.BaseUrl}/{_apiConfig.AccessToken}/{characterId}";

            var httpMock = new MockHttpMessageHandler();

            httpMock
                .When(requestUrl)
                .Respond(
                    HttpStatusCode.InternalServerError,
                    _responseFormat,
                    "{}");

            var httpClient = httpMock.ToHttpClient();
            httpClient.BaseAddress = new Uri(Url.Combine(_apiConfig.BaseUrl, _apiConfig.AccessToken, "/"));

            ISuperHeroHttpClient service = new SuperHeroHttpClientService(_apiConfig, httpClient);

            #endregion

            #region Act

            var response = await service.GetCharacter(characterId);

            #endregion

            #region Assert

            response.Should().NotBeNull();
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Code.Should().Be((int)ResultType.ThirdPartyError);
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Message.Should().Be(errorMessage);
            response.Should().BeOfType<Result<GetInfoResponse>>().Which.Results.Should().BeNull();

            #endregion
        }
    }
}
