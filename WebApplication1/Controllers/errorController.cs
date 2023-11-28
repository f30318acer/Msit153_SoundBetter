using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class errorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error404()
        { return View(); }

    }
}
