using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class RateController : Controller
    {
        public IActionResult Rate()
        {
            return View();
        }
    }
}
