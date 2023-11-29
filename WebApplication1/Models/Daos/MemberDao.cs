using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Models.Daos
{
    public class MemberDao:Controller
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
            var memInDb = _context.TMembers.FirstOrDefault(m=>m.FEmail==email);
            if (memInDb == null)
            {
                return null;
            }
            return memInDb;
        }
        public TMember GetMemberByPhone(string phone)
        {
            var memInDb = _context.TMembers.FirstOrDefault(m=>m.FPhone==phone);
            if(memInDb == null)
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
                FGender = vm.fGender, //看要不要設成bit
                FPermissionId = 1,
                FCreationTime = DateTime.Now,
                FBirthday = Convert.ToDateTime(vm.fBirthday)
            };
            _context.TMembers.Add(mem);
            _context.SaveChanges();
            if(vm.Photo!=null)
            {
                string fileName = $"MemberId_{mem.FMemberId}.jpg";
                mem.FPhotoPath = fileName ;
                string fphotoPath = Path.Combine(_environment.WebRootPath, "img/Member", fileName);
                using (var fileStream = new FileStream(fphotoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            _context.Update(mem);
            _context.SaveChanges();
        }
    }
}
