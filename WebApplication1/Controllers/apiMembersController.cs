using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using prjMusicBetter.Models;
using Microsoft.EntityFrameworkCore;

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
        //個人會員圖片 
        public IActionResult GetFMemberPhoto()
        {
            TMember member = _userInfoService.GetMemberInfo();
            var dto = _context.TMembers.Where(m=>m.FMemberId == member.FMemberId).Select(m => new { fphotoPath = m.FPhotoPath }).FirstOrDefault();
            return Json(dto);
        }
        public IActionResult UploadPhoto(IFormFile photo)
        {
            try
            {
                var mem = _userInfoService.GetMemberInfo();
                var memInDb = _context.TMembers.FirstOrDefault(m => m.FMemberId == mem.FMemberId);

                string fileName = $"FMemberId_{mem.FMemberId}.jpg";

                memInDb.FPhotoPath = fileName;
                _context.SaveChanges();

                string FPhotoPath = Path.Combine(_environment.WebRootPath, "img/Member", fileName);
                using(var fileStream = new FileStream(FPhotoPath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }

                var result = new ApiResult
                {
                    StatusCode = 200,
                    StatusMessage = "更新大頭貼成功",
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new ApiResult
                {
                    StatusCode = 500,
                    StatusMessage = "更新大頭貼失敗"
                };
                return Json(result);
            }
        }
        [HttpGet]
        public IActionResult QueryByMember(string fName)//MemberId
        {
            if (string.IsNullOrEmpty(fName))
            {
                return Json(new { success = false, message = "Name is null or empty" });

            }

            var member = _context.TMembers.Where(m => m.FName.Contains(fName)).ToList();
            if (!member.Any())
            {
                return Json(new { success = false, message = "No members found" });

            }

            return Json(new { success = true, data = member });


        }


    }
}
