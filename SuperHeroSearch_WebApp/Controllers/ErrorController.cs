using Microsoft.AspNetCore.Mvc;

namespace SuperHeroSearch_WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("{*url}", Order = 999)]
        public IActionResult NotFound404() => View();
    }
}
