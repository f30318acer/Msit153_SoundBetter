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
        public IActionResult Index (int memberId)
        {

            List<TMember> members =await _context.TMemberRelations.
                Where(m=>m.FMemberId == memberId && m.FMemberRelationStatusId==1).
                Select(m=>m.FRelationMemberId).ToListAsync();
            return View(members);
            
        }
    }
}
