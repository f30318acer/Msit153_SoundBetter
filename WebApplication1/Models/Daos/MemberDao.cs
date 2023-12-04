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
                                  
                                  where m.FMemberId== id//這裡不能只填ID 會抓不到
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
                                      FIntroduction = m.FIntroduction

                                  }).FirstOrDefault();
            return dto;
        }
        public void EditMember(FMemberEditVM vm)
        {
            var member = _context.TMembers.Where(m => m.FMemberId == vm.FMemberID).FirstOrDefault();
            if (vm.Photo != null)
            {
                string fileName = $"fMemberID_{member.FMemberId}.jpg";
                member.FPhotoPath = fileName;
                _context.SaveChanges();
                string photoPath = Path.Combine(_environment.WebRootPath, "img/Member", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }

            }
            member.FName = vm.FName;
            member.FPassword = vm.FPassword;
            member.FEmail = vm.FEmail;
            member.FPhone = vm.FPhone;
            _context.SaveChanges();

        }
        public void MemberPassword(MemberPasswordVM vm,int loginMemId)
        {
            var memberInDb = _context.TMembers.FirstOrDefault(m=>m.FMemberId==loginMemId);
            memberInDb.FPassword = vm.NewPassword;
            _context.SaveChanges();
        }
    }
}
