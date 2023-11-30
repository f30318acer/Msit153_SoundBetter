using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.Services;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{

    public class MembersController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IWebHostEnvironment _environment;
        MemberService _service;
        MemberDao _memberDao;


        public MembersController(dbSoundBetterContext context, UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
            _service = new MemberService(_context, _environment);
            _memberDao = new MemberDao(_context, _environment);

        }
        public IActionResult Index(string display)
        {
            ViewBag.Display = display;
            TMember member = _userInfoService.GetMemberInfo();
            var photo = (from m in _context.TMembers
                         where m.FMemberId == member.FMemberId
                         select new FMemberDto
                         {
                             FMemberID = member.FMemberId,
                             FPhotoPath = m.FPhotoPath,
                         }).FirstOrDefault();
            return View(photo);
        }
        public IActionResult  MemberInfo()
        {
            TMember member = _userInfoService.GetMemberInfo();
            FMemberDto mem = (from m in _context.TMembers
                              where m.FMemberId == member.FMemberId
                              select new FMemberDto
                              {
                                  FMemberID = member.FMemberId,
                                  FName = m.FName,
                                  FBirthday = Convert.ToDateTime(m.FBirthday).ToString("yyyy-MM-dd"),
                                  FEmail = m.FEmail,
                                  FPhone = m.FPhone,
                                  FGender = m.FGender ? "男" : "女",
                                  FPassword = m.FPassword,
                                  FPhotoPath = m.FPhotoPath,
                              }).FirstOrDefault();
            return PartialView(mem);
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult ProfileEdit()
        {
            return View();
        }
        public IActionResult CoolPon()
        {
            return View();
        }
        public async Task<IActionResult> Memberclass(int? id)
        {
            var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher).Where(t => t.FTeacherId == id);
            return View(await dbSoundBetterContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId");
            return View();
        }
    }
}
