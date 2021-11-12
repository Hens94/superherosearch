using FluentAssertions;
using HGUtils.Common.Enums;
using HGUtils.Common.Models;
using Moq;
using SuperHeroSearch_App.Contracts;
using SuperHeroSearch_App.Contracts.HttpClients;
using SuperHeroSearch_App.Models.HttpClients.Response;
using SuperHeroSearch_App.Services;
using SuperHeroSearch_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SuperHeroSearch_Test.Services
{
    public class SuperHeroServiceTests
    {
        [Fact]
        public async Task Search_ShouldReturnSuccess()
        {
            #region Arrange

            var searchName = "man";

            var serviceMock = new Mock<ISuperHeroHttpClient>();
            serviceMock.Setup(mock => mock.Search(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                new Result<SearchResponse>(
                    new SearchResponse
                    {
                        Response = "success",
                        ResultsFor = name,
                        Results = new List<HeroInfo>
                        {
                            new HeroInfo {
                                Id = "1",
                                Name = "Batman",
                                Powerstats = new Powerstats
                                {
                                    Intelligence =  "100",
                                    Strength = "26",
                                    Speed = "27",
                                    Durability = "50",
                                    Power = "47",
                                    Combat = "100"
                                },
                                Biography = new Biography
                                {
                                    Fullname = "Bruce Wayne",
                                    Alteregos = "No alter egos found.",
                                    Aliases = new string[] { "Insider", "Matches Malone" },
                                    PlaceOfBirth = "Crest Hill, Bristol Township; Gotham County",
                                    FirstAppearance = "Detective Comics #27",
                                    Publisher = "DC Comics",
                                    Alignment = "good"
                                },
                                Appearance = new Appearance
                                {
                                    Gender = "Male",
                                    Race = "Human",
                                    Height = new string[] { "6'2", "188 cm" },
                                    Weight = new string[] { "210 lb", "95 kg" },
                                    EyeColor = "blue",
                                    HairColor = "black"
                                },
                                Work = new Work
                                {
                                    Occupation = "Businessman",
                                    Base = "Batcave, Stately Wayne Manor, Gotham City; Hall of Justice, Justice League Watchtower"
                                },
                                Connections = new Connections
                                {
                                    GroupAffiliation = "Batman Family, Batman Incorporated, Justice League, Outsiders, Wayne Enterprises, Club of Heroes, formerly White Lantern Corps, Sinestro Corps",
                                    Relatives = ""
                                },
                                Image = new Image { Url = @"https://www.superherodb.com/pictures2/portraits/10/100/639.jpg" }
                            }
                        }
                    }));

            ISuperHero service = new SuperHeroService(serviceMock.Object);

            #endregion

            #region Act

            (var result, var error) = await service.Search(searchName);

            #endregion

            #region Assert

            error.Should().BeNull();
            result.Should().NotBeNull();
            result.Should().BeOfType<SearchResultsViewModel>().Which.Filter.Should().Be(searchName);
            result.Should().BeOfType<SearchResultsViewModel>().Which.Results.Should().NotBeEmpty();

            #endregion
        }

        [Fact]
        public async Task Search_ShouldReturnNotFound()
        {
            #region Arrange
            var searchName = "man not exists";
            var errorMessage = "character with given name not found";

            var serviceMock = new Mock<ISuperHeroHttpClient>();
            serviceMock.Setup(mock => mock.Search(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                new Result<SearchResponse>(
                    new SearchResponse
                    {
                        Response = "error",
                        Error = errorMessage
                    },
                    ResultType.ThirdPartyError,
                    errorMessage));

            ISuperHero service = new SuperHeroService(serviceMock.Object);

            #endregion

            #region Act

            (var result, var error) = await service.Search(searchName);

            #endregion

            #region Assert

            result.Should().BeNull();
            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorViewModel>().Which.Message.Should().Be(errorMessage);

            #endregion
        }

        [Fact]
        public async Task Search_ShouldReturnUncontrolledError()
        {
            #region Arrange
            var searchName = "man not exists";
            var errorMessage = "Uncontrolled Error";

            var serviceMock = new Mock<ISuperHeroHttpClient>();
            serviceMock.Setup(mock => mock.Search(It.IsAny<string>())).Throws<Exception>();

            ISuperHero service = new SuperHeroService(serviceMock.Object);

            #endregion

            #region Act

            (var result, var error) = await service.Search(searchName);

            #endregion

            #region Assert

            result.Should().BeNull();
            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorViewModel>().Which.Message.Should().Be(errorMessage);

            #endregion
        }

        [Fact]
        public async Task Find_ShouldReturnSuccess()
        {
            #region Arrange

            var characterId = "1";

            var serviceMock = new Mock<ISuperHeroHttpClient>();
            serviceMock.Setup(mock => mock.GetCharacter(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                new Result<GetInfoResponse>(
                    new GetInfoResponse
                    {
                        Response = "success",
                        Id = characterId,
                        Name = "Batman",
                        Powerstats = new Powerstats
                        {
                            Intelligence = "100",
                            Strength = "26",
                            Speed = "27",
                            Durability = "50",
                            Power = "47",
                            Combat = "100"
                        },
                        Biography = new Biography
                        {
                            Fullname = "Bruce Wayne",
                            Alteregos = "No alter egos found.",
                            Aliases = new string[] { "Insider", "Matches Malone" },
                            PlaceOfBirth = "Crest Hill, Bristol Township; Gotham County",
                            FirstAppearance = "Detective Comics #27",
                            Publisher = "DC Comics",
                            Alignment = "good"
                        },
                        Appearance = new Appearance
                        {
                            Gender = "Male",
                            Race = "Human",
                            Height = new string[] { "6'2", "188 cm" },
                            Weight = new string[] { "210 lb", "95 kg" },
                            EyeColor = "blue",
                            HairColor = "black"
                        },
                        Work = new Work
                        {
                            Occupation = "Businessman",
                            Base = "Batcave, Stately Wayne Manor, Gotham City; Hall of Justice, Justice League Watchtower"
                        },
                        Connections = new Connections
                        {
                            GroupAffiliation = "Batman Family, Batman Incorporated, Justice League, Outsiders, Wayne Enterprises, Club of Heroes, formerly White Lantern Corps, Sinestro Corps",
                            Relatives = ""
                        },
                        Image = new Image { Url = @"https://www.superherodb.com/pictures2/portraits/10/100/639.jpg" }
                    }));

            ISuperHero service = new SuperHeroService(serviceMock.Object);

            #endregion

            #region Act

            (var result, var error) = await service.Character(characterId);

            #endregion

            #region Assert

            error.Should().BeNull();
            result.Should().NotBeNull();
            result.Should().BeOfType<CharacterViewModel>().Which.Id.Should().Be(characterId);

            #endregion
        }

        [Fact]
        public async Task Find_ShouldReturnNotFound()
        {
            #region Arrange
            var characterId = "1";
            var errorMessage = "invalid id";

            var serviceMock = new Mock<ISuperHeroHttpClient>();
            serviceMock.Setup(mock => mock.GetCharacter(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                new Result<GetInfoResponse>(
                    new GetInfoResponse
                    {
                        Response = "error",
                        Error = errorMessage
                    },
                    ResultType.ThirdPartyError,
                    errorMessage));

            ISuperHero service = new SuperHeroService(serviceMock.Object);

            #endregion

            #region Act

            (var result, var error) = await service.Character(characterId);

            #endregion

            #region Assert

            result.Should().BeNull();
            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorViewModel>().Which.Message.Should().Be(errorMessage);

            #endregion
        }

        [Fact]
        public async Task Find_ShouldReturnUncontrolledError()
        {
            #region Arrange
            var characterId = "1";
            var errorMessage = "Uncontrolled Error";

            var serviceMock = new Mock<ISuperHeroHttpClient>();
            serviceMock.Setup(mock => mock.GetCharacter(It.IsAny<string>())).Throws<Exception>();

            ISuperHero service = new SuperHeroService(serviceMock.Object);

            #endregion

            #region Act

            (var result, var error) = await service.Character(characterId);

            #endregion

            #region Assert

            result.Should().BeNull();
            error.Should().NotBeNull();
            error.Should().BeOfType<ErrorViewModel>().Which.Message.Should().Be(errorMessage);

            #endregion
        }
    }
}
