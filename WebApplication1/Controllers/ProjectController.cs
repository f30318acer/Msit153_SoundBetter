using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
		public IActionResult Apply()
		{
			return View();
		}
	}
}
