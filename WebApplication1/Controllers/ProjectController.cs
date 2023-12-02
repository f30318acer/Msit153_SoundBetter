using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class ProjectController : Controller
    {
		private readonly dbSoundBetterContext _context;
		public ProjectController(dbSoundBetterContext context)
		{
			_context = context;
		}
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
            return View();
        }
    }
}
