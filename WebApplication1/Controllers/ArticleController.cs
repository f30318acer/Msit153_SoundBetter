using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class ArticleController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public ArticleController(dbSoundBetterContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Detail2()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = await _context.TArticles
                .Include(t => t.FMember)
                .Include(t => t.FStyle)
                .FirstOrDefaultAsync(m => m.FArticleId == id);
            if (tArticle == null)
            {
                return NotFound();
            }

            return View(tArticle);
        }

   
    }
}
