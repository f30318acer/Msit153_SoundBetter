using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using prjMusicBetter.Models;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using System.Text.Json;

namespace prjMusicBetter.Controllers
{
    public class apiHomeController : Controller
    {
        private readonly UserInfoService _userInfoService;
        private readonly dbSoundBetterContext _context;
        public apiHomeController(UserInfoService userInfoService, dbSoundBetterContext context)
        {
            _userInfoService = userInfoService;
            _context = context;
        }
        public IActionResult IsLogin()
        {
            bool isLogin = HttpContext.User.Identity.IsAuthenticated? true: false;

            return Json(IsLogin);
        }
        public IActionResult UpdateNav()
        {
            if (HttpContext.User.IsInRole("Member"))
            {
                TMember member = _userInfoService.GetMemberInfo();
                var dto = new
                {
                    Role = "Member",
                    datas = member
                };
                return Json(dto);
            }
            else
            {
                var dto = new
                {
                    Role = "None",
                    datas = "User not logged in or not a member"
                };
                return Json(dto);
            }
        }

    }
}
