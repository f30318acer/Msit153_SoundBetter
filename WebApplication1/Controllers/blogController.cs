using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class blogController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
