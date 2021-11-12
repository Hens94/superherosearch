using Microsoft.AspNetCore.Mvc;
using SuperHeroSearch_App.Contracts;
using SuperHeroSearch_App.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperHeroSearch_WebApp.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ISuperHero _superHero;

        public CharacterController(ISuperHero superHero)
        {
            _superHero = superHero;
        }

        private IActionResult ProcessResponse<TResult>(
            (TResult, ErrorViewModel) response,
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

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            if (name is null)
            {
                return View();
            }

            return ProcessResponse(
                await _superHero.Search(name));
        }

        [HttpGet("character/{id}")]
        public async Task<IActionResult> Find(string id)
        {
            return ProcessResponse(
                await _superHero.Character(id),
                error =>
                    error.Message.Equals("invalid id") ?
                    RedirectToAction("NotFound404", "Error") :
                    null);
        }
    }
}
