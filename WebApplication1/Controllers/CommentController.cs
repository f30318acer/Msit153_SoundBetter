using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Dtos.Comment;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using System.Xml.Linq;

namespace prjMusicBetter.Controllers
{
    public class CommentController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;

        public CommentController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(int articleId)
        {
            //確認會員是否有登入
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null) { return RedirectToAction("Login", "Home"); }
            //取得_文章所有留言資料
            articleId = 20;
            var comments = _context.TComments.Include(e => e.FMember).Where(e => e.FArticleId == articleId).ToList();
            var viewModel = new CommentListViewModel()
            {
                Comments = comments,
            };

            return View(viewModel);

        }


        public IActionResult Create()
        { return View(); }
        [HttpPost]
        public IActionResult Create(CommentDto dto)
        {

            dto.ArticleId = 20;
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

        public IActionResult Edit(int? id)
        {
            dbSoundBetterContext db = new dbSoundBetterContext();
            TComment x = db.TComments.FirstOrDefault(p => p.FCommentId == id);
            if (x != null)
                return RedirectToAction("List");
            return View();
        }
        [HttpPost]
        public IActionResult Edit(TComment pIN)
        {
            dbSoundBetterContext db = new dbSoundBetterContext();
            TComment pDB = db.TComments.FirstOrDefault(p => p.FCommentId == pIN.FCommentId);
            if (pDB != null)
            {
                pDB.FMemberId = pIN.FMemberId;
                pDB.FArticleId = pIN.FArticleId;
                pDB.FCommentContent = pIN.FCommentContent;
                pIN.FCommentTime = DateTime.Now;

                db.SaveChanges();
            }
            return RedirectToAction("List");



        }

        public IActionResult Delete(int? id)
        {
            dbSoundBetterContext db = new dbSoundBetterContext();
            TComment x = db.TComments.FirstOrDefault(p => p.FCommentId == id);
            if (x != null)
            {
                db.TComments.Remove(x);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
