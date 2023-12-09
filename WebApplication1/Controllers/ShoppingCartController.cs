using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{
	public class ShoppingCartController : Controller
	{
		public IActionResult List()
		{
            // 取得Session

            // 依照Session內 ProductId 檢查 => 
            // 例如：商品特價、商品沒貨、
            // 

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
