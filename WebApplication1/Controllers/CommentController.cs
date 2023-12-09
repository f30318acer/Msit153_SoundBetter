using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class CommentController : Controller
    {
        private readonly dbSoundBetterContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(int id, string myComment)
        {

            //TComment comment = new TComment();

            //var q = from m in _context.TComments
            //        where m.FArticleId == comment.FArticleId
            //        select new TComment
            //        {
            //            FArticleId = 20,
            //            FMemberId = id,
            //            FCommentContent = myComment,
            //            FCommentTime = DateTime.Now
            //        };

            //_context.Add(comment);
            //return RedirectToAction("List");

            return View();
        }
    }
}
