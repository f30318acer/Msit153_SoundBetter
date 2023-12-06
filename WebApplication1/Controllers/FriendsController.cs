using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace prjMusicBetter.Controllers
{

    public class FriendsController : Controller
    {
        private readonly UserInfoService _userInfoService;
        private readonly dbSoundBetterContext _context;
        public FriendsController(UserInfoService userInfoService, dbSoundBetterContext context)
        {
            _userInfoService = userInfoService;
            _context = context;
        }
        public async Task<IActionResult> Index(int memberId)
        {

            var friendsList = await _context.TMemberRelations.
                Where(m => m.FMemberId == memberId && m.FMemberRelationStatusId == 1).
                Select(m => m.FRelationMemberId)
                .ToListAsync();

            var friendMembers = await _context.TMembers
               .Where(m => friendsList.Contains(m.FMemberId)) // 这里假设 Id 是成员的唯一标识符
               .ToListAsync();

            return View(friendMembers);

        }
    }
}
