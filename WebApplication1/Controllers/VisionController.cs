using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;

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

        public IActionResult upload()
        { return View(); }
        //[HttpPost]
        //public IActionResult upload(TVision v)
        //{
        //    string fileName = "";

        //    if (v != null)
        //    {

        //        if (v. > 0)
        //            fileName = Path.GetFileName(film.Content);
        //        var path = Path.Combine
        //        (Server.MapPath("~/Vision/film"), fileName);
        //        v.SaveAs(path);

        //    }

        //    return RedirectToAction("Showfilm");
        //}
    }
}
