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

        //public IActionResult Edit(int? id)
        //{
        //    dbSoundBetterContext db = new dbSoundBetterContext();
        //    TComment x = db.TComments.FirstOrDefault(p => p.FMemberId == id);
        //    if (x != null)
        //        return RedirectToAction("List");
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Edit(TComment pIN)
        //{
        //    dbSoundBetterContext db = new dbSoundBetterContext();
        //    TComment pDB = db.TComments.FirstOrDefault(p => p.FCommentId == pIN.FCommentId);
        //    if (pDB != null)
        //    {
        //        pDB.FMemberId = pIN.FMemberId;
        //        pDB.FArticleId = pIN.FArticleId;
        //        pDB.FCommentContent = pIN.FCommentContent;
        //        pIN.FCommentTime = DateTime.Now;

        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.TComments == null) { return NotFound(); }
            var tComment = await _context.TComments.FindAsync(id);
            if (tComment == null) { return NotFound(); }

            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle", tComment.FArticleId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tComment.FMemberId);

            return View(tComment);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FCommentId,FMenberId,FArticleId,FCommentContent,FCommentTime")] TComment tComment)
        {
            if (id != tComment.FCommentId) { return NotFound(); }
            if (ModelState.IsValid)
            {
                try 
                {
                    _context.Update(tComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TCommentExitsts(tComment.FCommentId)) { return NotFound(); }
                    else { throw; }
                }
                return RedirectToAction(nameof(List));
            }
            ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle", tComment.FArticleId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tComment.FMemberId);

            return View(tComment);

        }



        //public IActionResult Delete(int? id)
        //{
        //    dbSoundBetterContext db = new dbSoundBetterContext();
        //    TComment x = db.TComments.FirstOrDefault(p => p.FCommentId == id);
        //    if (x != null)
        //    {
        //        db.TComments.Remove(x);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TComments == null)
            {
                return NotFound();
            }

            var tComment = await _context.TComments
                .Include(t => t.FArticle)
                .Include(t => t.FMember)
                .FirstOrDefaultAsync(m => m.FCommentId == id);
            if (tComment == null)
            {
                return NotFound();
            }

            return View(tComment);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TComments == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TComments'  is null.");
            }
            var tComment = await _context.TComments.FindAsync(id);
            if (tComment != null)
            {
                _context.TComments.Remove(tComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
        private bool TCommentExitsts(int id)
        {
            return (_context.TComments?.Any(e=>e.FCommentId==id)).GetValueOrDefault();
        }


    }
}
