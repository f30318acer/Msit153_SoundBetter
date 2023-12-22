using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using System.Formats.Asn1;

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
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                // 處理會員資料不存在的情況，導向登入頁面
                return RedirectToAction("Login", "Home"); // 導向登入頁面
            }
            if (member != null)
            {
                ViewBag.MemberId = member.FMemberId;
            }
            /*var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher);
            return View(await dbSoundBetterContext.ToListAsync());*/
            return View();
        }


        /*=======課程內頁===============*/

        public async Task<IActionResult> Viewclass(int? id)
        {
            ViewBag.Id = id;
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                // 處理會員資料不存在的情況，導向登入頁面
                return RedirectToAction("Login", "Home"); // 導向登入頁面
            }
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

            var DealClass = _context.TDealClassDetails.Where(t => t.FClassId == tClass.FClassId).Select(t => t.FMemberId).Any(id => id == member.FMemberId);
            ViewBag.DealClass = DealClass;//我有沒有買這堂課

            return View(tClass);
		}

        


        /*=======新增課程===============*/

        public IActionResult Create()
        {
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                // 處理會員資料不存在的情況，導向登入頁面
                return RedirectToAction("Login", "Home"); // 導向登入頁面
            }
            ViewBag.MemberId = member.FMemberId;
            return View();
        }





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


        /*=======學生一覽===============*/

        public IActionResult student(int? id)
        {
            // 找出TDealClassDetails表中符合FClassId == id的所有FMemberId
            var DealClass = _context.TDealClassDetails
                                    .Where(d => d.FClassId == id)
                                    .OrderBy(d => d.FDealClassDetailId)
                                    .Select(d => d.FMemberId)
                                    .ToList();

            if (DealClass != null && DealClass.Any())
            {
                // 在TMembers表中找出FMemberId等於DealClass的學生資訊
                var students = _context.TMembers
                                      .Where(m => DealClass.Contains(m.FMemberId))
                                      .ToList();
                return View(students);
            }
            else { return NotFound(); }
        }
        //是否有學生
        public IActionResult CheckDealClass(int id)
        {
            var isClassIdIncluded = _context.TDealClassDetails.Any(d => d.FClassId == id);
            return Json(isClassIdIncluded);
        }

        /*=======學生編輯===============*/

        public IActionResult studentEdit(int? id)
        {
            // 找出TDealClassDetails表中符合FClassId == id的所有FMemberId
            var DealClass = _context.TDealClassDetails
                                    .Where(d => d.FClassId == id)
                                    .OrderBy(d => d.FDealClassDetailId)
                                    .Select(d => d.FMemberId)
                                    .ToList();

            ViewBag.Id = id;//課程id

            if (DealClass != null && DealClass.Any())
            {
                // 在TMembers表中找出FMemberId等於DealClass的學生資訊
                var students = _context.TMembers
                                      .Where(m => DealClass.Contains(m.FMemberId))
                                      .ToList();
                return View(students);
            }
            else {
                return RedirectToAction("NoStudentsError");
            }
        }
        //沒有學生時
        public IActionResult NoStudentsError()
        {
            return View();
        }
    }
}
