using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
   
    
    public class apiMembersController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao;
        private readonly UserInfoService _userInfoService;
        public apiMembersController(dbSoundBetterContext context, IWebHostEnvironment environment, UserInfoService userInfoService)
        {
            _context = context;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
            _userInfoService = userInfoService;        
        }
        public IActionResult GetFMemberPhoto()
        {
            TMember member = _userInfoService.GetMemberInfo();
            var dto = _context.TMembers.Where(m=>m.FMemberId == member.FMemberId).Select(m => new { fphotoPath = m.FPhotoPath }).FirstOrDefault();
            return Json(dto);
        }








        public IActionResult Index()
        {
            return View();
        }
    }
}
