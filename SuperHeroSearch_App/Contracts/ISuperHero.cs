using SuperHeroSearch_App.ViewModels;
using System.Threading.Tasks;

namespace SuperHeroSearch_App.Contracts
{
    public interface ISuperHero
    {
        Task<(SearchResultsViewModel, ErrorViewModel)> Search(string name);
        Task<(CharacterViewModel, ErrorViewModel)> Character(string id);
    }
}
