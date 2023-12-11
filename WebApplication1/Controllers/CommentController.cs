using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Dtos.Comment;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{
    public class CommentController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public CommentController(dbSoundBetterContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(int articleId)
        {
            //取得_當前文章所有留言資料
            articleId = 20;
            var comments = _context.TComments.Include(e => e.FMember).Where(e => e.FArticleId == articleId).ToList();
            var viewModel = new CommentListViewModel()
            {
                Comments = comments,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CommentDto dto)
        {
            //新增_文章備註資料
            _context.TComments.Add(new TComment()
            {
                FMemberId = dto.MemberId,
                FCommentContent = dto.Content,
                FArticleId = dto.ArticleId,
                FCommentTime = DateTime.Now,
            });
            _context.SaveChanges();

            return Ok();
        }
    }
}
