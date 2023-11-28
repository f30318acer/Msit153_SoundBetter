using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class MembersController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public MembersController(dbSoundBetterContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Profile1()
        {
            return View();
        }
        public IActionResult ProfileEdit()
        {
            return View();
        }
        public IActionResult CoolPon()
        {
            return View();
        }
        public async Task<IActionResult> Memberclass(int? id)
        {
            var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher).Where(t => t.FTeacherId == id);
            return View(await dbSoundBetterContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId");
            return View();
        }
    }
}
