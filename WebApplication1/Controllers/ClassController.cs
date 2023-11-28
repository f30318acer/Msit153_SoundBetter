using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class ClassController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public ClassController(dbSoundBetterContext context)
        {
            _context = context;
        }
        /*=======課程首頁===============*/

        //public IActionResult Index()
        //      {
        //          return View();
        //      }
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        /*=======課程內頁===============*/

        public async Task<IActionResult> Viewclass(int? id)
        {
            if (id == null || _context.TClasses == null)
            {
                return NotFound();
            }

            var tClass = await _context.TClasses.FindAsync(id);
            if (tClass == null)
            {
                return NotFound();
            }
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tClass.FSiteId);
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClass.FTeacherId);

            if (id <= (_context.TClasses.Count() - 2))
            {
                var Preclass = _context.TClasses.Where(t => t.FClassId == (id + 1)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.PrClass = Preclass;//下一個課程名稱
                var Senclass = _context.TClasses.Where(t => t.FClassId == (id + 2)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.SenClass = Senclass;//下下一個課程名稱
            }
            else
            {
                var Preclass = _context.TClasses.Where(t => t.FClassId == (id - 1)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.PrClass = Preclass;//上一個課程名稱
                var Senclass = _context.TClasses.Where(t => t.FClassId == (id - 2)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.SenClass = Senclass;//上上一個課程名稱
            }
            ViewBag.AllClass = _context.TClasses.Count();//有多少課程

            var Introduction = _context.TMembers.Where(t => t.FMemberId == tClass.FTeacherId).Select(t => t.FIntroduction).SingleOrDefault();
            ViewBag.teacher = Introduction;//教師自述

            return View(tClass);
        }




        /*=======新增課程===============*/

        //public IActionResult Createclass()
        //{
        //    return View();
        //}

        public IActionResult Create()
        {
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId");
            //ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FClassId,FTeacherId,FClassName,FPrice,FDescription,FStartdate,FEnddate,FSiteId,FThumbnailPath")] TClass tClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId", tClass.FSiteId);
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClass.FTeacherId);
            return View(tClass);
        }
    }
}
