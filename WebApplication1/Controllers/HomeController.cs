using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using prjMusicBetter.Models;
using System.Diagnostics;
using WebApplication1.Models;
using System.Security.Claims;
using prjMusicBetter.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authentication.Cookies;




namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbSoundBetterContext _context;

        public HomeController(ILogger<HomeController> logger, dbSoundBetterContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            Dictionary<string, Func<RedirectToActionResult>> roleActions = new Dictionary<string, Func<RedirectToActionResult>>
            {

            };

            foreach(var role in roleActions.Keys)
            {
                if(HttpContext.User.IsInRole(role))
                {
                    return roleActions[role]();
                }
            }
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(LoginVM vm, string? returnUrl)
        //{
        //    //VM表單驗證
        //    if (ModelState.IsValid == false)
        //    {
        //        return View(vm);
        //    }

        //    //todo
        //    //vm.password 進行雜湊 再去比對

        //    var member = _context.TMembers.FirstOrDefault(m=>m.FEmail==vm.Email &&m.FPasswod==vm.Password);

        //    //輸入錯誤
        //    if(member ==null)
        //    {
        //        ModelState.AddModelError("", "帳號密碼錯誤!");
        //        return View(vm);
        //    }

        //    //登入
        //    if(member != null)
        //    {
        //        List<Claim> claims = new List<Claim>
        //        {
        //            new Claim("fMemberID",member.FMemberId.ToString()),
        //            new Claim(ClaimTypes.Role,"Member"),
        //        };

        //        ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

               
        //    }
        //}


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

            };

            foreach (var role in roleActions.Keys)
            {
                if(HttpContext.User.IsInRole(role))
                {
                    return roleActions[role]();
                }
            }
                return View();
        }

		public ActionResult Register()
		{

            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId");
            return View();         
		}
        public IActionResult test()
        {
            return View();
        }
        public IActionResult NoLogin()
        {
            return View();
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
    }
}