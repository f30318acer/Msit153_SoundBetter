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
        public void Register(FMemberVM vm)
        {
            var mem = new TMember
            {
                FName = vm.fName,
                FPassword = vm.fPassword,
                FEmail = vm.fEmail,
                FPhone = vm.fPhone,
                FGender=vm.fGender, //看要不要設成bit
                FCreationTime = DateTime.Now,
                FBirthday = Convert.ToDateTime(vm.fBirthday),

            };
            //_context.TMembers.Add(mem);
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
