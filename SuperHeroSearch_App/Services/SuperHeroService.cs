using HGUtils.Helpers.Common;
using SuperHeroSearch_App.Contracts;
using SuperHeroSearch_App.Contracts.HttpClients;
using SuperHeroSearch_App.Mappings;
using SuperHeroSearch_App.ViewModels;
using System.Threading.Tasks;

namespace SuperHeroSearch_App.Services
{
    public class SuperHeroService : ISuperHero
    {
        private readonly ISuperHeroHttpClient _superHeroHttpClient;

        public SuperHeroService(ISuperHeroHttpClient superHeroHttpClient)
        {
            _superHeroHttpClient = superHeroHttpClient;
        }

        public async Task<(CharacterViewModel, ErrorViewModel)> Character(string id)
        {
            try
            {
                var infoResponse = await _superHeroHttpClient.GetCharacter(id);

                if (!infoResponse.IsSuccess())
                {
                    return (null, new ErrorViewModel(ErrorType.Warning, infoResponse.Message));
                }

                return (infoResponse.Results.ToInfoViewModel(), null);
            }
            catch
            {
                return (null, new ErrorViewModel(ErrorType.Error, "Uncontrolled Error"));
            }
        }

        public async Task<(SearchResultsViewModel, ErrorViewModel)> Search(string name)
        {
            try
            {
                var searchResponse = await _superHeroHttpClient.Search(name);

                if (!searchResponse.IsSuccess())
                {
                    return (null, new ErrorViewModel(ErrorType.Warning, searchResponse.Message));
                }

                return (searchResponse.Results.ToSearchResultsViewModel(), null);
            }
            catch
            {
                return (null, new ErrorViewModel(ErrorType.Error, "Uncontrolled Error"));
            }
        }
    }
}
