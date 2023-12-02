using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Models.Daos
{
    public class MemberDao : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;
        public MemberDao(dbSoundBetterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public TMember GetMemberByEmail(string email)
        {
            var memInDb = _context.TMembers.FirstOrDefault(m => m.FEmail == email);
            if (memInDb == null)
            {
                return null;
            }
            return memInDb;
        }
        public TMember GetMemberByPhone(string phone)
        {
            var memInDb = _context.TMembers.FirstOrDefault(m => m.FPhone == phone);
            if (memInDb == null)
            {
                return null;
            }
            return memInDb;
        }
        //註冊頁面有甚麼就填寫甚麼
        public void Register(FMemberVM vm)
        {
            var mem = new TMember
            {
                FUsername = vm.fUsername,
                FName = vm.fName,
                FPassword = vm.fPassword,
                FEmail = vm.fEmail,
                FPhone = vm.fPhone,
                FGender = Convert.ToBoolean(vm.fGender),
                FPermissionId = 2,//一般會員執行寫2
                FCreationTime = DateTime.Now,
                FBirthday = Convert.ToDateTime(vm.fBirthday)
            };
            _context.TMembers.Add(mem);
            _context.SaveChanges();
            if (vm.Photo != null)
            {
                string fileName = $"MemberId_{mem.FMemberId}.jpg";
                mem.FPhotoPath = fileName;
                string fphotoPath = Path.Combine(_environment.WebRootPath, "img/Member", fileName);
                using (var fileStream = new FileStream(fphotoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            _context.Update(mem);
            _context.SaveChanges();
        }
        public FMemberEditDto GetFMemberById(int id)
        {
            FMemberEditDto dto = (from m in _context.TMembers
                                  where m.FMemberId == id
                                  select new FMemberEditDto
                                  {
                                      FMemberID = m.FMemberId,
                                      FPhotoPath = m.FPhotoPath,
                                      FName = m.FName,
                                      FPassword = m.FPassword,
                                      FBirthday = Convert.ToDateTime(m.FBirthday).ToString("yyyy-MM-dd"),
                                      FEmail = m.FEmail,
                                      FPhone = m.FPhone,
                                      FGender = m.FGender ? "女" : "男",
                                      FUsername = m.FUsername,
                                      FCreationTime = Convert.ToDateTime(m.FCreationTime).ToString("yyyy-MM-dd"),
                                      FIntroduction = m.FIntroduction,
                                      FPermissionId = m.FPermissionId
                                  }).FirstOrDefault();
            return dto;
        }
    }
}
