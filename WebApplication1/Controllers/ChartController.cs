using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using prjMusicBetter.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;

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


       public IActionResult GenderRatio()
        {

            var genderCount = _context.TMembers
                .GroupBy(M=>M.FGender)
                .Select(group => new
                {
                    Gender = group.Key ? "Male" : "Female",
                    Count = group.Count()
                })
                .ToList();
            return Json(genderCount);
        }
    }
}
