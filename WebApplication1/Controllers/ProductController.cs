using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
