using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.Dtos.Comment;
using prjMusicBetter.Models.infra;
using System.ComponentModel.Design;
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                // 處理會員資料不存在的情況，導向登入頁面
                return RedirectToAction("Login", "Home"); // 導向登入頁面
            }
            if (member != null)
            {
                ViewBag.MemberId = member.FMemberId;
            }
            return View();
        }
        public IActionResult Detail2()
        {
            return View();

            //留言部分
			//await _context.TArticles.Include(c => c.TComments).FirstOrDefaultAsync(m => m.FArticleId == Id);
		}
        public IActionResult Create()
        {
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                // 處理會員資料不存在的情況，導向登入頁面
                return RedirectToAction("Login", "Home"); // 導向登入頁面
            }
            if (member != null)
            {
                ViewBag.MemberId = member.FMemberId;
            }
            return View();
        }


   
        public async Task<IActionResult> Details(int? id)
        { 
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            //原本的CODE
            //var tArticle = await _context.TArticles
            //    .Include(t => t.FMember)
            //    .Include(t => t.FStyle)
            //    .FirstOrDefaultAsync(m => m.FArticleId == id);

            var tArticle = await _context.TArticles
              .Include(t => t.FMember)
              .Include(t => t.FStyle)
              .Include(c => c.TComments) //此行為下方留言功能用
              .FirstOrDefaultAsync(m => m.FArticleId == id);
            //

            //強制Load資料庫 不然留言會出現匿名會員項目
            _context.TComments.Load();
            _context.TMembers.Load();


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


            //留言部分
            //await _context.TArticles.Include(c => c.TComments).FirstOrDefaultAsync(m => m.FArticleId == id);
            ViewData["UserName"] = _userInfoService.GetMemberInfo().FUsername;
            ViewData["UserPhoto"] = _userInfoService.GetMemberInfo().FPhotoPath;
            ViewData["UserId"] = _userInfoService.GetMemberInfo().FMemberId;





            var query = from comment in _context.TComments
                        join member in _context.TMembers on comment.FMemberId equals member.FMemberId
                        select new
                        {   
                            MemberUserName = member.FUsername,
                            MemberPhotoPath = member.FPhotoPath
                        };

            var result = await query.ToListAsync();

            foreach (var commentermemberInfo in result)
            {
                var commenterUserName = commentermemberInfo.MemberUserName;
                TempData["CommenterUserName"] = commenterUserName;
                var commenterPhotoPath = commentermemberInfo.MemberPhotoPath;
                TempData["CommenterPhoto"] = commenterPhotoPath;
            }
             
            //留言部分

            var StyleId = _context.TStyles.Where(t => t.FStyleId == tArticle.FStyleId).Select(t => t.FName).SingleOrDefault();
            ViewBag.StyleId = StyleId;//類型

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


        ///評論留言功能
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int id, CommentDto cdto)
        {
            try
            {
                // 目前使用者ID
                cdto.FMemberId = _userInfoService.GetMemberInfo().FMemberId;

                // 文章ID
                var article = await _context.TArticles
                    .Include(t => t.FMember)
                    .Include(t => t.FStyle)
                    .Where(e => e.FArticleId == id)
                    .SingleOrDefaultAsync();

                if (article != null)
                {
                    cdto.FArticleId = article.FArticleId;

                    // 加入留言
                    _context.TComments.Add(new TComment()
                    {
                        FCommentId = cdto.FCommentId,
                        FMemberId = cdto.FMemberId,
                        FCommentContent = cdto.FCommentContent,
                        FArticleId = cdto.FArticleId,
                        FCommentTime = DateTime.Now,
                    });

                    // 儲存
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = id });
                }
                else
                {
                    // 文章未找到
                    return NotFound("Article not found");
                }
            }
            catch (Exception ex)
            {
                // 異常狀況
                return StatusCode(500, "An error occurred while adding the comment.");
            }
        }

        //使用者自刪評論留言
        public IActionResult DeleteComment(int? id,int? commentId, int? articleId)
        {
            TComment comment = _context.TComments.FirstOrDefault(p => p.FCommentId == commentId);
            TComment article = _context.TComments.FirstOrDefault(q => q.FArticleId ==  articleId);
            if (comment != null)
            {
                _context.TComments.Remove(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", new { id = articleId });
        }




    }
}
