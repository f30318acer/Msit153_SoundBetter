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
        public void FMemberEdit(FMemberEditVM vm)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.FEmail);
            if (memInEmail != null && memInEmail.FMemberId != vm.FMemberID)
            {
               throw new Exception("此信箱已被其他會員註冊，無法修改!");
            }
            var memInPhone = _dao.GetMemberByPhone(vm.FPhone);
            if (memInPhone != null && memInPhone.FMemberId != vm.FMemberID)
            {
                throw new Exception("已有此電話號碼!");
            }
            if(vm.FPhone.Length>10||vm.FPhone.Length < 10)
            {
                throw new Exception("電話號碼為10碼!");
            }
            _dao.EditMember(vm);
        }
    }
}
