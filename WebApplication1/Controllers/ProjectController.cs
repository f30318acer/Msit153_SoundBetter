using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;

namespace prjMusicBetter.Controllers
{
    public class ProjectController : Controller
    {
		private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        public ProjectController(dbSoundBetterContext context, UserInfoService userInfoService)
		{
			_context = context;
            _userInfoService = userInfoService;

        }
        [Authorize(Roles = "Member")]
        public IActionResult List()
        {
            return View();
        }
		public IActionResult Apply()
		{
			return View();
		}		
		public async Task<IActionResult> Details(int? id)
		{

			if (id == null || _context.TProjects == null)
			{
				return NotFound();
			}

			var tProject = await _context.TProjects
				.Include(t => t.FMember)
				.Include(t => t.FProjectStatus)
				.Include(t => t.FSite)
				.Include(t => t.FSiteNavigation)
				.FirstOrDefaultAsync(m => m.FProjectId == id);
			if (tProject == null)
			{
				return NotFound();
			}

			return View(tProject);
		}
		public IActionResult ApplyConfirm()
		{
			return View();
		}
        public IActionResult Create()
        {
            TMember member = _userInfoService.GetMemberInfo();
            ViewBag.MemberId = member.FMemberId;
            return View();
        }
    }
}
