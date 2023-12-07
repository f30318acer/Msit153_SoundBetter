using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.IO;

namespace prjMusicBetter.Controllers
{
    public class VisionController : Controller
    {   


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult List()
        {
            return View();
        }

        public IActionResult update()
        { return View(); }
        //[HttpPost]
        //public IActionResult update(HttpPostedFileBase film)
        //{
        //    string fileName = "";

        //    if (film != null)
        //    {

        //        if (film.ContentLength > 0)
        //            fileName = Path.GetFileName(film.Content);
        //        var path = Path.Combine
        //        (Server.MapPath("~/Vision/film"), fileName);
        //        film.SaveAs(path);

        //    }

        //    return RedirectToAction("Showfilm");
        //}
    }
}
