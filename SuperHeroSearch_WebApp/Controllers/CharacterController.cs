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

        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            if (name is null)
            {
                return View();
            }

            (var result, var error) = await _superHero.Search(name);

            if (error is not null)
            {
                TempData["ErrorType"] = error.Type;
                TempData["ErrorMessage"] = error.Message;

                return View();
            }

            return View(result);
        }
    }
}
