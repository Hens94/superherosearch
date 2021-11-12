using SuperHeroSearch_App.Models.HttpClients.Response;
using SuperHeroSearch_App.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SuperHeroSearch_App.Mappings
{
    public static class SuperHeroMappings
    {
        public static SearchResultsViewModel ToSearchResultsViewModel(this SearchResponse response)
        {
            if (!response.Results.Any()) return null;

            return new SearchResultsViewModel
            {
                Filter = response.ResultsFor,
                Results = response.Results.ToInfoViewModelList()
            };
        }

        public static IEnumerable<CharacterViewModel> ToInfoViewModelList(this IEnumerable<HeroInfo> response)
        {
            foreach (var info in response)
            {
                yield return info.ToInfoViewModel();
            }
        }

        public static CharacterViewModel ToInfoViewModel(this HeroInfo response)
        {
            if (response is null) return null;

            return new CharacterViewModel
            {
                Id = response.Id,
                Name = response.Name,
                Image = response.Image.Url,
                Powerstats = new CharacterViewModel.CharacterPowerstats
                {
                    Intelligence = response.Powerstats.Intelligence,
                    Strength = response.Powerstats.Strength,
                    Speed = response.Powerstats.Speed,
                    Durability = response.Powerstats.Durability,
                    Power = response.Powerstats.Power,
                    Combat = response.Powerstats.Combat
                },
                Biography = new CharacterViewModel.CharacterBiography
                {
                    Fullname = response.Biography.Fullname,
                    Alteregos = response.Biography.Alteregos,
                    Aliases = response.Biography.Aliases,
                    PlaceOfBirth = response.Biography.PlaceOfBirth,
                    FirstAppearance = response.Biography.FirstAppearance,
                    Publisher = response.Biography.Publisher,
                    Alignment = response.Biography.Alignment
                },
                Appearance = new CharacterViewModel.CharacterAppearance
                {
                    Gender = response.Appearance.Gender,
                    Race = response.Appearance.Race,
                    Height = response.Appearance.Height,
                    Weight = response.Appearance.Weight,
                    EyeColor = response.Appearance.EyeColor,
                    HairColor = response.Appearance.HairColor
                },
                Work = new CharacterViewModel.CharacterWork
                {
                    Occupation = response.Work.Occupation,
                    Base = response.Work.Base
                },
                Connections = new CharacterViewModel.CharacterConnections
                {
                    GroupAffiliation = response.Connections.GroupAffiliation,
                    Relatives = response.Connections.Relatives
                }
            };
        }
    }
}
