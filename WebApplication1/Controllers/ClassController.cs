using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class ClassController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public ClassController(dbSoundBetterContext context)
        {
            _context = context;
        }
        /*=======課程首頁===============*/

        //public IActionResult Index()
        //      {
        //          return View();
        //      }
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TClasses.Include(t => t.FSite).Include(t => t.FTeacher);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        /*=======課程內頁===============*/

        public IActionResult Viewclass()
		{
			return View();
		}




        /*=======課程內頁===============*/

        public IActionResult Createclass()
        {
            return View();
        }
    }
}
