using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.Services;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{
    public class MemberPageController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IWebHostEnvironment _environment;
        MemberService _service;
        MemberDao _memberDao;

        public MemberPageController(dbSoundBetterContext context, UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
            _service = new MemberService(_context, _environment);
            _memberDao = new MemberDao(_context, _environment);
        }
        //View
        public IActionResult List()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            TMember member = _userInfoService.GetMemberInfo();
            ViewBag.Fav = 0;
            ViewBag.MemberId = 0;
            if (member != null)
            {
                ViewBag.MemberId = member.FMemberId;
                TMemberRelation fav = _context.TMemberRelations.FirstOrDefault(f => f.FMemberId == member.FMemberId && f.FRelationMemberId == id);
                if (fav != null && fav.FMemberRelationStatusId == 1)
                {
                    ViewBag.Fav = 1;
                }
            }

            if (id == null || _context.TMembers == null)
            {
                return NotFound();
            }

            var tProject = await _context.TMembers
                .FirstOrDefaultAsync(m => m.FMemberId == id);
            if (tProject == null)
            {
                return NotFound();
            }

            return View(tProject);
        }
        //API
        //===List_All===
        public IActionResult ListAll()
        {
            var dbSoundBetterContext = _context.TMembers;
            return Json(dbSoundBetterContext);
        }
        //===GetMemberSkill===
        public IActionResult GetMemberSkill(int id)
        {
            var dbSoundBetterContext = from s in _context.TMemberSkills
                                       join n in _context.TSkills
                                       on s.FSkillId equals n.FSkillId
                                       where s.FMemberId == id
                                       select new { FSkillId=s.FSkillId, FName=n.FName, FYearExp=s.FYearExp, FDescription=s.FDescription };
            return Json(dbSoundBetterContext.ToArray());
        }
        //===GetMemberClass===
        public IActionResult GetMemberClass(int id)
        {
            var dbSoundBetterContext = _context.TClasses.Where(c => c.FTeacherId == id);
            return Json(dbSoundBetterContext);
        }
        public IActionResult GetMemberPrj(int id)
        {
            var dbSoundBetterContext = _context.TProjects.Where(c => c.FMemberId == id && c.FProjectStatusId == 1);
            return Json(dbSoundBetterContext);
        }
        //===GetMemberWorks===
        public IActionResult GetMemberWorks(int id)
        {
            var dbSoundBetterContext = _context.TWorks.Where(c => c.FMemberId == id);
            return Json(dbSoundBetterContext);
        }
        //===GetFallower===
        public IActionResult GetFallower(int id)
        {
            var fallower = from f in _context.TMemberRelations
                           join m in _context.TMembers
                           on f.FMemberId equals m.FMemberId
                           where f.FRelationMemberId == id && f.FMemberRelationStatusId == 1
                           select m;
            return Json(fallower);
        }
        //===GetMemberArticle===
        public IActionResult GetMemberArticle(int id)
        {
            var dbSoundBetterContext = _context.TArticles.Where(c => c.FMemberId == id);
            return Json(dbSoundBetterContext);
        }
        //===新增追蹤===
        [HttpPost]
        public IActionResult favMember(int id, TMemberRelation fav)
        {
            if (fav != null)
            {
                TMemberRelation fDb = _context.TMemberRelations.FirstOrDefault(f => f.FMemberId == fav.FMemberId && f.FRelationMemberId == fav.FRelationMemberId);
                if (fDb == null)
                {
                    DateTime now = DateTime.Now;
                    fav.FRelationMemberId = id;
                    fav.FMemberRelationStatusId = 1;
                    _context.Add(fav);
                    _context.SaveChanges();
                    return Content("追蹤成功");
                }
                else {
                    fDb.FMemberRelationStatusId = 1;
                    _context.SaveChanges();
                    return Content("追蹤成功");
                }
            }
            return Content("錯誤");
        }
        //===取消追蹤===
        [HttpPost]
        public IActionResult DisFavMember(TMemberRelation fav)
        {
            TMemberRelation fDb = _context.TMemberRelations.FirstOrDefault(f => f.FMemberId == fav.FMemberId && f.FRelationMemberId == fav.FRelationMemberId);
            if (fDb != null)
            {
                fDb.FMemberRelationStatusId = 2;
                _context.SaveChanges();
                return Content("取消追蹤");
            }
            return Content("錯誤");
        }
        //幫layout讀取通知
        public IActionResult LoadNotifi()
        {
            TMember member = _userInfoService.GetMemberInfo();
            if (member != null)
            {
                List<TNotification> notis = _context.TNotifications.Where(n => n.FMemberId == member.FMemberId && n.FNotifiStatus == 1).ToList();
                if (notis.Count > 0) {
                    return Json(notis);
                }                
            }
            return Json(new List<TNotification> { new TNotification { FNotification = "沒有通知", FProjectId = 0, FClassId = 0 } });
        }
    }
}
