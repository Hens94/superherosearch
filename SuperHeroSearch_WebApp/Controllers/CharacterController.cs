using Microsoft.AspNetCore.Mvc;
using SuperHeroSearch_App.Contracts;
using SuperHeroSearch_App.ViewModels;
using SuperHeroSearch_WebApp.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroSearch_WebApp.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ISuperHero _superHero;

        public SearchResultsViewModel SearchResult
        {
            get => HttpContext.Session.GetObjectFromJson<SearchResultsViewModel>("search");
            set => HttpContext.Session.SetObjectAsJson("search", value);
        }

        public CharacterViewModel CharacterResult
        {
            get => HttpContext.Session.GetObjectFromJson<CharacterViewModel>("character");
            set => HttpContext.Session.SetObjectAsJson("character", value);
        }

        public CharacterController(ISuperHero superHero)
        {
            _superHero = superHero;
        }

        private IActionResult ProcessResponse<TResult>(
            (TResult, ErrorViewModel) response,
            string sessionKey,
            Func<ErrorViewModel, IActionResult> aditionalValidation = null) where TResult : class
        {
            (var result, var error) = response;

            if (error is not null)
            {
                if (aditionalValidation is not null)
                {
                    var returnAction = aditionalValidation(error);

                    if (returnAction is not null) return returnAction;
                }

                TempData["ErrorType"] = error.Type;
                TempData["ErrorMessage"] = error.Message;

                return View();
            }

            HttpContext.Session.SetObjectAsJson(sessionKey, result);

            return View(result);
        }

        [HttpGet]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "name" }, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index(string name)
        {
            if (SearchResult is not null &&
                (SearchResult.Results?.Any() ?? false) &&
                (SearchResult.Filter?.Equals(name) ?? false))
            {
                return View(SearchResult);
            }
            else
            {
                SearchResult = new SearchResultsViewModel { Filter = name };
            }

            if (name is null)
            {
                return View();
            }

            return ProcessResponse(await _superHero.Search(name), "search");
        }

        [HttpGet("character/{id}")]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "id" }, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Find(string id)
        {
            if (CharacterResult is not null && (CharacterResult.Id?.Equals(id) ?? false))
            {
                return View(CharacterResult);
            }

            return ProcessResponse(
                await _superHero.Character(id),
                "character",
                error =>
                    error.Message.Equals("invalid id") ?
                    RedirectToAction("NotFound404", "Error") :
                    null);
        }
    }
}
