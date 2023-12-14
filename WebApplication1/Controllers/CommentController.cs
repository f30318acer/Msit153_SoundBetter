using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Dtos;
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
        private IWebHostEnvironment _environment = null;

        public CommentController(dbSoundBetterContext context, UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
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
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CommentDto dto)
        {

            dto.FArticleId = 20;


            _context.TComments.Add(new TComment()
            {
                FMemberId = dto.FMemberId,
                FCommentContent = dto.FCommentContent,
                FArticleId = dto.FArticleId,
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

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.TComments == null) { return NotFound(); }
        //    var tComment = await _context.TComments.FindAsync(id);
        //    if (tComment == null) { return NotFound(); }

        //    tComment.FCommentTime = DateTime.Now;
        //    ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle", tComment.FArticleId);
        //    ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tComment.FMemberId);
        //    ViewData["FCommentTime"] = new SelectList(_context.TComments, "FCommentTime", "FCommentTime", tComment.FCommentTime);

        //    return View(tComment);

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FCommentId,FMenberId,FArticleId,FCommentContent,FCommentTime")] TComment tComment)
        //{
        //    if (id != tComment.FCommentId) { return NotFound(); }
        //    if (ModelState.IsValid)
        //    {
        //        try 
        //        {
        //            _context.Update(tComment);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TCommentExitsts(tComment.FCommentId)) { return NotFound(); }
        //            else { throw; }
        //        }
        //        return RedirectToAction(nameof(List));
        //    }
        //    tComment.FCommentTime = DateTime.Now;
        //    ViewData["FArticleId"] = new SelectList(_context.TArticles, "FArticleId", "FArticle", tComment.FArticleId);
        //    ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tComment.FMemberId);
        //    ViewData["FCommentTime"] = new SelectList(_context.TComments, "FCommentTime", "FCommentTime", tComment.FCommentTime);
        //    return View(tComment);

        //}

        public IActionResult Edit(int? id)
        {
            
            TComment comment = _context.TComments.FirstOrDefault(p => p.FCommentId == id);
            if (comment == null)
            { return RedirectToAction("List"); }
            return View(comment);
        }
        [HttpPost]
        public IActionResult Edit(TComment pIn)
        {

            if (ModelState.IsValid)
            {
                TComment pDb = _context.TComments.FirstOrDefault(p => p.FCommentId == pIn.FCommentId);



                if (pDb != null)
                {
                pDb.FMemberId = pIn.FMemberId;
                pDb.FArticleId = pIn.FArticleId;
                pDb.FCommentContent = pIn.FCommentContent;
                pDb.FCommentTime = DateTime.Now;
                _context.SaveChanges();
                }



                return RedirectToAction("List");
            
            }

            return View(pIn);
        }



        public IActionResult Delete(int? id)
        {
            TComment comment = _context.TComments.FirstOrDefault(p => p.FCommentId == id);
            if (comment != null)
            {
                _context.TComments.Remove(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }



        
        private bool TCommentExitsts(int id)
        {
            return (_context.TComments?.Any(e=>e.FCommentId==id)).GetValueOrDefault();
        }


    }
}
