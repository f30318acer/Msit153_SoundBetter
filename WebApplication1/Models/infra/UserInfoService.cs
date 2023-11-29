using prjMusicBetter.Models.EFModels;
namespace prjMusicBetter.Models.infra
{
    public class UserInfoService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserInfoService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public TMember GetMemberInfo()
        {
            var claim = _contextAccessor.HttpContext.User.Claims.ToList();
            var memberId = claim.Where(c => c.Type == "MemberId").FirstOrDefault();
            if(memberId == null)
            {
                return null;
            }

            int id = Convert.ToInt32(memberId.Value);
            TMember member = new dbSoundBetterContext().TMembers.FirstOrDefault(m => m.FMemberId == id);
            return (member);
        }
    }
}
