using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class apiMemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
