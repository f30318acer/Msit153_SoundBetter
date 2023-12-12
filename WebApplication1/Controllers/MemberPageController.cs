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
        //===GetMemberWorks===
        public IActionResult GetMemberWorks(int id)
        {
            var dbSoundBetterContext = _context.TWorks.Where(c => c.FMemberId == id);
            return Json(dbSoundBetterContext);
        }
        //===GetMemberArticle===
        public IActionResult GetMemberArticle(int id)
        {
            var dbSoundBetterContext = _context.TArticles.Where(c => c.FMemberId == id);
            return Json(dbSoundBetterContext);
        }
    }
}
