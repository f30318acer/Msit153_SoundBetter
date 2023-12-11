using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class MusicShareController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        public MusicShareController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;

        }
        public IActionResult List()
        {

            return View();
        }

        public IActionResult Details()
        {

            TMember member = _userInfoService.GetMemberInfo();
            ViewBag.MemberId = 0;
            if (member != null)
            {
                ViewBag.MemberId = member.FMemberId;
            }
            return View();
        }

    }
}
