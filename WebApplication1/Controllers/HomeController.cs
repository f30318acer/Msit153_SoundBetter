using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        // GET:登入頁面

        public ActionResult Login()
        {
            return View();
        }
        // GET:登入失敗頁面
        public ActionResult NoLogin()
        {
            return View();
        }
        // GET:忘記密碼頁面
        public ActionResult ForgetPwd()
        {
            return View();
        }
        // GET:註冊頁面
        public ActionResult Register()
		{
			return View();
		}
        public IActionResult test()
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