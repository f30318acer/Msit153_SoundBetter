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




		/*=======新增課程===============*/

		public IActionResult Createclass()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Createclass(TProduct p)
        {
            DbDemoContext db = new DbDemoContext();
            db.TProducts.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");


            // 若是 Required(ErrorMessage = "名稱是必填欄位") 顯示錯誤
            // 則把jquery版本更新至最新 < script src = "~/Scripts/jquery-3.4.1.min.js" >
        }
    }
}
