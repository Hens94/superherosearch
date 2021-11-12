using System.Collections.Generic;

namespace SuperHeroSearch_App.ViewModels
{
    public class SearchResultsViewModel
    {
        public string Filter { get; set; }
        public IEnumerable<CharacterViewModel> Results { get; set; }
    }
}
