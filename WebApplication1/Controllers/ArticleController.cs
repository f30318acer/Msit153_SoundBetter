using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
	public class ArticleController : Controller
	{
		public IActionResult List()
		{
			return View();
		}
	}
}
