using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
    public class ClassController : Controller
    {
		/*=======課程首頁===============*/

		public IActionResult Index()
        {
            return View();
        }




		/*=======課程內頁===============*/

		public IActionResult Viewclass()
		{
			return View();
		}




		/*=======課程內頁===============*/

		public IActionResult Createclass()
		{
			return View();
		}
	}
}
