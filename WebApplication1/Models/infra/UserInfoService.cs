using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.ViewModels;


namespace prjMusicBetter.Models.infra
{
    public class UserInfoService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly dbSoundBetterContext _Context;
        public UserInfoService(IHttpContextAccessor contextAccessor, dbSoundBetterContext context)
        {
            _contextAccessor = contextAccessor;
            _Context = context;
        }
        public TMember GetMemberInfo()
        {
            var claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var memberId = claim.Where(c => c.Type == "fMemberID").FirstOrDefault();

            if(memberId == null)
            {
                return null;
            }

            int id = Convert.ToInt32(memberId.Value);

            TMember member=_Context.TMembers.FirstOrDefault(m=>m.FMemberId == id);

            return member;
        }

        /// <summary>
        /// 取得使用者Id
        /// </summary>
        public int GetMemberId()
        {
            var claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var memberId = claim.Where(c => c.Type == "fMemberID").FirstOrDefault();


            if (memberId == null)
            {
                return 0;
            }

            int id = Convert.ToInt32(memberId.Value);

            return id;
        }
        public int GetCouponId()
        {
            var claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var Couponid = claim.Where(c => c.Type == "fCouponID").FirstOrDefault();


            if (Couponid == null)
            {
                return 0;
            }

            int id = Convert.ToInt32(Couponid.Value);

            return id;
        }

    }
}
