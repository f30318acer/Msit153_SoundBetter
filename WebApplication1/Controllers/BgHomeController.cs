using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
	public class BgHomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Chart()
		{
            return View();
        }
}
}
