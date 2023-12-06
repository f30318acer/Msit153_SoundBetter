using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;

namespace prjMusicBetter.Controllers
{
    public class ClassController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        public ClassController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;//抓使用者
        }
        /*=======課程首頁===============*/
        public IActionResult Index()
        {
            /*var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher);
            return View(await dbSoundBetterContext.ToListAsync());*/
            return View();
        }


        /*=======課程內頁===============*/

        public async Task<IActionResult> Viewclass(int? id)
        {
            await _context.SaveChangesAsync();
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

            var classClick = _context.TClassClicks.FirstOrDefault(c => c.FClassId == id);
            if (classClick != null)
            {
                classClick.FClick++;//點閱數+1
            }
            // 保存變更到資料庫
            await _context.SaveChangesAsync();

            /*if (id <= (_context.TClasses.Count() - 2))
            {
                var nextClass = _context.TClasses
                    .Where(t => t.FClassId > tClass.FClassId)
                    .OrderBy(t => t.FClassId)
                    .FirstOrDefault();

                if (nextClass != null)
                {
                    ViewBag.PrClass = nextClass.FClassName; // 下一個課程名稱
                    ViewBag.PrePath = nextClass.FThumbnailPath; // 下一個課程圖片位址
                }

                var secondNextClass = _context.TClasses
                    .Where(t => t.FClassId > tClass.FClassId)
                    .OrderBy(t => t.FClassId)
                    .Skip(1)
                    .FirstOrDefault();

                if (secondNextClass != null)
                {
                    ViewBag.SenClass = secondNextClass.FClassName; // 下下一個課程名稱
                    ViewBag.SenPath = secondNextClass.FThumbnailPath; // 下下一個課程圖片位址
                }
            }
            else
            {
                var prevClass = _context.TClasses
                    .Where(t => t.FClassId < tClass.FClassId)
                    .OrderByDescending(t => t.FClassId)
                    .FirstOrDefault();

                if (prevClass != null)
                {
                    ViewBag.PrClass = prevClass.FClassName; // 上一個課程名稱
                    ViewBag.PrePath = prevClass.FThumbnailPath; // 上一個課程圖片位址
                }

                var secondPrevClass = _context.TClasses
                    .Where(t => t.FClassId < tClass.FClassId)
                    .OrderByDescending(t => t.FClassId)
                    .Skip(1)
                    .FirstOrDefault();

                if (secondPrevClass != null)
                {
                    ViewBag.SenClass = secondPrevClass.FClassName; // 上上一個課程名稱
                    ViewBag.SenPath = secondPrevClass.FThumbnailPath; // 上上一個課程圖片位址
                }
            }*/
            ViewBag.AllClass = _context.TClasses.Count();//有多少課程

            ViewBag.SkillId = tClass.FSkillId;//類型

            var Name = _context.TMembers.Where(t => t.FMemberId == tClass.FTeacherId).Select(t => t.FName).SingleOrDefault();
            ViewBag.Name = Name;//教師自述

            var Introduction = _context.TMembers.Where(t => t.FMemberId == tClass.FTeacherId).Select(t => t.FIntroduction).SingleOrDefault();
            ViewBag.teacher = Introduction;//教師自述

            var teacherimg = _context.TMembers.Where(t => t.FMemberId == tClass.FTeacherId).Select(t => t.FPhotoPath).SingleOrDefault();
            ViewBag.teacherimg = teacherimg;//教師圖片

            var skillname = _context.TSkills.Where(t => t.FSkillId == tClass.FSkillId).Select(t => t.FName).SingleOrDefault();
            ViewBag.skillname = skillname;//類型

            var SiteName = _context.TSites.Where(t => t.FSiteId == tClass.FSiteId).Select(t => t.FSiteName).SingleOrDefault();
            ViewBag.SiteName = SiteName;//場地

            var Address = _context.TSites.Where(t => t.FSiteId == tClass.FSiteId).Select(t => t.FAddress).SingleOrDefault();
            ViewBag.Address = Address;//地點

            return View(tClass);
		}

        


        /*=======新增課程===============*/

        public IActionResult Create()
        {
            TMember member = _userInfoService.GetMemberInfo();
            ViewBag.MemberId = member.FMemberId;
            /*ViewData["FSiteId"] = new SelectList(_context.TSites, "FSiteId", "FSiteId");
            ViewData["FTeacherId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");*/
            return View();
        }
        /*[HttpPost]
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
        }*/





        /*=======修改課程===============*/

        public async Task<IActionResult> Edit(int? id)
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

            ViewBag.SiteId = tClass.FSiteId;//原本的地址

            return View(tClass);
        }

        public IActionResult classFav(int? id)
        {
            var classfav = _context.TClassFavs.Where(m => m.FClassId == id).Select(t => t.FMemberId);
            return Json(classfav);//這堂課有誰喜歡
        }
    }
}
