using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{
	public class ShoppingCartController : Controller
	{
		public IActionResult List()
		{
			return View();
		}
        public IActionResult Checkout()
        {
            return View();
        }
		//[HttpPost]
  //      public IActionResult List(ShoppingCartVM vm)
  //      {
  //          return RedirectToAction("Checkout");
  //      }
    }
}
