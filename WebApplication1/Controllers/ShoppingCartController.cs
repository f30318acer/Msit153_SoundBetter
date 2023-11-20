using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
	public class ShoppingCartController : Controller
	{
		public IActionResult List()
		{
			return View();
		}
	}
}
