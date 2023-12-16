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
        public IActionResult workclick()
        {
            return View();
        }
        public IActionResult CombinedClicks()
        {
            return View("CombinedClicks");

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
        public IActionResult GetClassClicksData()
        {
            // 首先，從 TClasses 表中選擇所有類別名稱和ID
            var classInfo = _context.TClasses
                .Select(c => new { c.FClassId, c.FClassName })
                .ToList();

            // 接著，從 TClassClick 表中選擇類別ID和對應的點擊次數
            var clickInfo = _context.TClassClicks
                .GroupBy(cc => cc.FClassId)
                .Select(g => new { FClassId = g.Key, FClick = g.Sum(x => x.FClick) })
                .ToList();

            // 然後，將這兩個列表聯接在一起，以 FClassId 為聯接條件
            var classClicksData = classInfo
                .Join(clickInfo,
                    c => c.FClassId,
                    cc => cc.FClassId,
                    (c, cc) => new
                    {
                        c.FClassName,
                        cc.FClick
                    })
                .ToList();

            // 最後，將結果轉換為您需要的格式，這裡假設您需要兩個分開的列表
            List<object> data = new List<object>
    {
        classClicksData.Select(x => x.FClassName).ToList(),
        classClicksData.Select(x => x.FClick).ToList()
    };

            return Json(data); // 將數據包裝成 JSON 格式返回


        }
        [HttpPost]
        public List<object> GetWorksClicksData()
        {
            List<object> data = new List<object>();

            List<string> works = _context.TWorks.Select(w=>w.FWorkName).ToList();

            data.Add(works);

            List<int> ClicksWorks = _context.TWorks.Select(c =>c.FClick).ToList();
            data.Add(ClicksWorks);
            return data;

        }

    }
}
