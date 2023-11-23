using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
