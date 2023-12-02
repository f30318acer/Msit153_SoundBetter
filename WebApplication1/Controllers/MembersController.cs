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
            if (member == null)
            {
                return RedirectToAction("error");
            }
            var photo = (from m in _context.TMembers
                         where m.FMemberId == member.FMemberId
                         select new FMemberDto
                         {
                             FMemberID = member.FMemberId,
                             FPhotoPath = m.FPhotoPath,
                         }).FirstOrDefault();
            return View(photo);
        }
        public IActionResult MemberInfo()
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
                                  FGender = (bool)m.FGender ? "男" : "女",
                                  FPassword = m.FPassword,
                                  FPhotoPath = m.FPhotoPath,
                              }).FirstOrDefault();
            return PartialView(mem);
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult MemberInfoEdit(int id)
        {
            var dto = _memberDao.GetFMemberById(id);

            if (dto != null)
            {
                FMemberEditVM vm = new FMemberEditVM()
                {
                    FMemberID = dto.FMemberID,
                    FName = dto.FName,
                    FUsername = dto.FUsername,
                    FBirthday = dto.FBirthday,
                    FEmail = dto.FEmail,
                    FGender = dto.FGender,
                    FPhone = dto.FPhone
                };
                return PartialView(vm);
            }
            else
            {
                // 處理 dto 為 null 的情況
                // 比如重定向到錯誤頁面或顯示一個錯誤消息
                return RedirectToAction("ErrorPage"); // 或者 return View("ErrorView");
            }    
        }
        //public IActionResult MemberInfoEdit(FMemberEditVM vm)
        //{
            
        //}
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
