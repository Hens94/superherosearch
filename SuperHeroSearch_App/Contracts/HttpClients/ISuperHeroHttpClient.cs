using HGUtils.Common.Interfaces;
using SuperHeroSearch_App.Models.HttpClients.Response;
using System.Threading.Tasks;

namespace SuperHeroSearch_App.Contracts.HttpClients
{
    public interface ISuperHeroHttpClient
    {
        Task<IResult<SearchResponse>> Search(string name);
        Task<IResult<GetInfoResponse>> GetCharacter(string id);
    }
}
