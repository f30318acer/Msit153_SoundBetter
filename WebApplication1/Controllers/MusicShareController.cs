using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class MusicShareController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
