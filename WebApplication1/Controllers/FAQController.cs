using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FAQ() 
        { return View();}
    }
}
