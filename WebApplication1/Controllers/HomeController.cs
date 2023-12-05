using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjMusicBetter.Models;
using System.Diagnostics;
using WebApplication1.Models;
using System.Security.Claims;
using prjMusicBetter.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;
using prjMusicBetter.Models.infra;
using Microsoft.AspNetCore.Authorization;
using prjMusicBetter.Models.Daos;
using System.Text;
using System.Text.Json;
using prjMusicBetter.Models.Services;




namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserInfoService _userInfoService;
        MemberDao _dao;
        MemberService _service;


        public HomeController(dbSoundBetterContext context,ILogger<HomeController> logger,IWebHostEnvironment environment ,UserInfoService userInfoService)
        {
            _logger = logger;
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;            
            _dao = new MemberDao(_context, _environment);
            _service = new MemberService(_context, _environment);
        }
       
        public IActionResult Index()
        { 
            return View();
        }
        public IActionResult BackgroundIndex()
        {
            return View();
        }
        public IActionResult Vision()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            Dictionary<string, Func<RedirectToActionResult>> roleActions = new Dictionary<string, Func<RedirectToActionResult>>
            {
                 {"Administrator",()=>RedirectToAction("Index", "BgHome") }, // 假設管理員角色是 "Administrator"
                 {"Member",()=>RedirectToAction("Index", "Home")} // 假設一般會員角色是 "Member"
            };

            foreach (var role in roleActions.Keys)
            {
                if (HttpContext.User.IsInRole(role))
                {
                    return roleActions[role]();
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl)
        {
            //VM表單驗證
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }

            //todo
            //vm.password 進行雜湊 再去比對

            var member = _context.TMembers
                .FirstOrDefault(m => m.FEmail == vm.Email && m.FPassword == vm.Password);

            if (member == null)
            {
                ModelState.AddModelError("", "帳號密碼錯誤!");
                return View(vm);
            }



            //建立用戶身分宣告
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,member.FName),
                new Claim("fMemberID",member.FMemberId.ToString()),
                new Claim(ClaimTypes.Role,member.FPermissionId==1 ? "Administrator":"Member"),     
            };
            ClaimsIdentity identity =new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal =new ClaimsPrincipal(identity);

            //執行登入
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

            //確定登入完後提供 歡迎回來 登入者fUserName

            TempData["AlertLogin"] = member.FName;

            //依據用戶角色重定向倒不同頁面
            if (member.FPermissionId == 1)
            {
                return RedirectToAction("index", "Bghome");
            }
            else
            {
                //一般會員重定向到首頁
                
                return RedirectToAction("index", "Home");
                
            }
        }


        public IActionResult Register()
        {
            //ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId");
            return View();
        }

        [HttpPost]
        public IActionResult Register(FMemberVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                _service.MemberResgister(vm);
                TempData["AlertRegister"] = vm.fName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","新增失敗,"+ex.Message);
                return View(vm);
            }
            return RedirectToAction("Index");
         }

        //============================================
        //已登入用戶名稱
        [Authorize]
        public IActionResult Test()
        {
            return Content(_userInfoService.GetMemberInfo().FName);
        }
        
        public IActionResult NoLogin()
        {
            return View("尚未登入");
        }
        //會員登出功能
        public async Task<IActionResult> Loginout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult ForgetPwd()
        {
            return View();
        }




        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult ERROR()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFoundPG()
        {
            return View();
        }

        public IActionResult aboutUS() { return View(); }
        public IActionResult TermOfTeacher() { return View(); }
        public IActionResult Term() { return View(); }

        //public IActionResult JoinUS() { return View(); }
        //public IActionResult Partner() { return View(); }
        public IActionResult contactUS() { return View(); }


    }
}
