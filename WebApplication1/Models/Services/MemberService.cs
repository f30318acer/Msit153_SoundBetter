using Humanizer;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.ViewModels;


namespace prjMusicBetter.Models.Services
{
    public class MemberService
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao;
        public MemberService(dbSoundBetterContext Context, IWebHostEnvironment environment)
        {
            _context = Context;
            _environment = environment;
            _dao = new MemberDao(_context,_environment);
        }
       public void MemberResgister(FMemberVM vm)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.fEmail);
            var memInPhone = _dao.GetMemberByPhone(vm.fPhone);
            if(memInEmail != null)
            {
                throw new Exception("已有此信箱!");
            }
            if (vm.fPhone.Length > 10 || vm.fPhone.Length < 0)
            {
                throw new Exception("電話號碼為10碼");
            }
            if(memInPhone != null)
            {
                throw new Exception("已有此電話號碼");
            }
            if(Convert.ToDateTime(vm.fBirthday)>=DateTime.Now)
            {
                throw new Exception("輸入生日必須大於今日");
            }
            _dao.Register(vm);
        }
    }
}
