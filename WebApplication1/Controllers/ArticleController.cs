using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;

namespace prjMusicBetter.Controllers
{
    public class ArticleController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;

        public ArticleController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;
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
        
            if (id <= (_context.TArticles.Count() - 2))
            {
                var Prearticle = _context.TArticles.Where(t => t.FArticleId == (id + 1)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.PrArticle = Prearticle;//下一個課程名稱
                var PrePath = _context.TArticles.Where(t => t.FArticleId == (id + 1)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.PrePath = PrePath;//下一個課程圖片位址
                var Senarticle = _context.TArticles.Where(t => t.FArticleId == (id + 2)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.SenArticle = Senarticle;//下下一個課程名稱
                var SenPath = _context.TArticles.Where(t => t.FArticleId == (id + 2)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.SenPath = SenPath;//下下一個課程圖片位址
            }
            else
            {
                var Prearticle = _context.TArticles.Where(t => t.FArticleId == (id - 1)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.PrArticle = Prearticle;//上一個課程名稱
                var PrePath = _context.TArticles.Where(t => t.FArticleId == (id - 1)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.PrePath = PrePath;//上一個課程圖片位址
                var Senarticle = _context.TArticles.Where(t => t.FArticleId == (id - 2)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.SenArticle = Senarticle;//上上一個課程名稱
                var SenPath = _context.TArticles.Where(t => t.FArticleId == (id - 2)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.SenPath = SenPath;//上一個課程圖片位址
            }
            ViewBag.AllArticle = _context.TArticles.Count();//有多少課程



            return View(tArticle);
          
        }
        public async Task<IActionResult> ViewArticle(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }
            var tArticle = await _context.TArticles.FindAsync(id);
            if (tArticle == null)
            {
                return NotFound();
            }
            if (id <= (_context.TArticles.Count() - 2))
            {
                var Prearticle = _context.TArticles.Where(t => t.FArticleId == (id + 1)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.PrArticle = Prearticle;//下一個課程名稱
                var PrePath = _context.TArticles.Where(t => t.FArticleId == (id + 1)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.PrePath = PrePath;//下一個課程圖片位址
                var Senarticle = _context.TArticles.Where(t => t.FArticleId == (id + 2)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.SenArticle = Senarticle;//下下一個課程名稱
                var SenPath = _context.TArticles.Where(t => t.FArticleId == (id + 2)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.SenPath = SenPath;//下下一個課程圖片位址
            }
            else
            {
                var Prearticle = _context.TArticles.Where(t => t.FArticleId == (id - 1)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.PrArticle = Prearticle;//上一個課程名稱
                var PrePath = _context.TArticles.Where(t => t.FArticleId == (id - 1)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.PrePath = PrePath;//上一個課程圖片位址
                var Senarticle = _context.TArticles.Where(t => t.FArticleId == (id - 2)).Select(t => t.FTitle).SingleOrDefault();
                ViewBag.SenArticle = Senarticle;//上上一個課程名稱
                var SenPath = _context.TArticles.Where(t => t.FArticleId == (id - 2)).Select(t => t.FPhotoPath).SingleOrDefault();
                ViewBag.SenPath = SenPath;//上一個課程圖片位址
            }
            ViewBag.AllArticle = _context.TArticles.Count();//有多少課程

            

            return View(tArticle);    
        }

    }
}
