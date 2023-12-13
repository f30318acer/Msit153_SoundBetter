using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Services;


namespace prjMusicBetter.Controllers
{
    public class ChatController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IWebHostEnvironment _environment;
        //private readonly UserManager<TMember> _userManager;

        public ChatController(dbSoundBetterContext context, UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
