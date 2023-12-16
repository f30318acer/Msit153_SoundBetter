using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using prjMusicBetter.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;
using prjMusicBetter.Models.ViewModels;


namespace prjMusicBetter.Controllers
{
    public class ChartController : Controller
    {

        private readonly dbSoundBetterContext _context;
        public ChartController(dbSoundBetterContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult classclick()
        {
            return View();
        }    
        public IActionResult GenderRatio()
        {

            var genderCount = _context.TMembers
                .GroupBy(M => M.FGender)
                .Select(group => new
                {
                    Gender = group.Key ? "Male" : "Female",
                    Count = group.Count()
                })
                .ToList();
            return Json(genderCount);
        }
        [HttpPost]
        public List<object> GetClassClicksData()
        {
            List<object> data = new List<object>();

            List<string> classes = _context.SalesDa


            //var classClicksData = _context.TClasses
            //    .Join(_context.TClassClicks,
            //          c => c.FClassId,
            //          cc => cc.FClassId,
            //          (c, cc) => new { c.FClassName, cc.FClick })
            //    .GroupBy(x => x.FClassName)
            //    .Select(group => new
            //    {
            //        ClassName = group.Key,
            //        TotalClicks = group.Sum(x => x.FClick)
            //    })
            //    .OrderBy(x => x.ClassName)
            //    .ToList();

            //return Json(classClicksData);
        }

    }
}
