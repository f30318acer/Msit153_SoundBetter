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
                fGender=vm.fGender,
                fCreationTime = DateTime.Now,
                fBirthday = Convert.ToDateTime(vm.fBirthday),

            };
        }
    }
}
