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
    [Authorize(Roles = "Member")]
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
            //if (member == null)
            //{
            //    return RedirectToAction("error");
            //}
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
                                  FGender = m.FGender ? "男" : "女",
                                  FPassword = m.FPassword,
                                  FPhotoPath = m.FPhotoPath,
                              }).FirstOrDefault();
            return PartialView(mem);
        }
        public IActionResult MemberInfoEdit(int id)
        {
            var dto = _memberDao.GetFMemberById(id); 
            if (dto != null)
            {
                FMemberEditVM vm = new FMemberEditVM()
                {
                    FMemberID = dto.fMemberID,
                    FName = dto.fName,
                    FUsername = dto.fUsername,
                    FBirthday = dto.fBirthday,
                    FEmail = dto.fEmail,
                    FGender = dto.fGender,
                    FPhone = dto.fPhone
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
        [HttpPost]
        public IActionResult MemberInfoEdit(FMemberEditVM vm)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                result = new ApiResult { StatusCode = 500, StatusMessage = errors.FirstOrDefault() };
                return Json(result);
            }
            var member = _context.TMembers.FirstOrDefault(m=>m.FMemberId == vm.FMemberID);
            try
            {
                _service.FMemberEdit(vm);
                result = new ApiResult { StatusCode = 200, StatusMessage = "編輯資料成功!" };
                return Json(result);
            }
            catch (Exception ex)
            {
                result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message };
                return Json(result);
            }
        }
        
        public IActionResult CoolPon()
        {
            return View();
        }

        //public async Task<IActionResult> Memberclass(int? id)
		public IActionResult Memberclass()
		{
			TMember member = _userInfoService.GetMemberInfo();
            ViewBag.MemberId = member.FMemberId;
			return View();
		}
        public IActionResult Create()
        {
            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId");
            return View();
        }
        public IActionResult MemberPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult MemberPassword(MemberPasswordVM vm)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                result = new ApiResult { StatusCode=500,StatusMessage=errors.FirstOrDefault() };
                return Json(result);
            }
            try
            {
                int loginMemId = _userInfoService.GetMemberInfo().FMemberId;
                _service.MemberPasswordReset(vm, loginMemId);
                result = new ApiResult { StatusCode = 200, StatusMessage = "編輯資料成功!" };
                return Json(result);

            }
            catch (Exception ex)
            {
                result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message };
                return Json(result);

            }
        }
        public IActionResult MemberMyKeep()
        {
            TMember member = _userInfoService.GetMemberInfo();
            return PartialView(member);
        }
        public IActionResult Friends()
        {
            TMember member = _userInfoService.GetMemberInfo();

            // 查询数据库以获取会员的好友列表数据
            var friendList = _context.TMemberRelations.Where
                (m => m.FMemberId == member.FMemberId && m.FMemberRelationStatusId == 1)
                .Select(m => m.FMemberRelationId).ToList();

            var viewModel = new FriendsViewModel
            {
                Member = member,
                Friends = friendList


            };

            return View(viewModel);
        }
    }
}
