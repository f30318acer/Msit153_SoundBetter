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
            var mem = new Members
            {
                fName = vm.fName,
                fPassword = vm.fPassword,
                fEmail = vm.fEmail,
                fPhone = vm.fPhone,
                fGender=vm.fGender, //看要不要設成bit
                fCreationTime = DateTime.Now,
                fBirthday = Convert.ToDateTime(vm.fBirthday),

            };
            //_context.TMembers.Add(mem);
            _context.SaveChanges();
            if(vm.Photo!=null)
            {
                string fileName = $"MemberId_{mem.fMemberID}.jpg";
                mem.fPhotoPath = fileName ;
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
