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

        //取得_文章所有留言資料



        //public IActionResult List(int? articleId, int commentId)
        //{
        //    確認會員是否有登入
        //    TMember member = _userInfoService.GetMemberInfo();
        //    if (member == null) { return RedirectToAction("Login", "Home"); }



        //    articleId = 20;
        //    var comments = _context.TComments
        //                .Include(e => e.FMember)
        //                .Where(e => e.FArticleId == articleId.Value).ToList();
        //    .Include(e => e.FCommentId)
        //    .Where(e => e.FCommentId == commentId).ToList();
        //    var viewModel = new CommentListViewModel()
        //    {
        //        Comments = comments,
        //    };

        //    return View(viewModel);



        //}

        public IActionResult List(int? articleId)
        {

               TMember member = _userInfoService.GetMemberInfo();
               if (member == null) { return RedirectToAction("Login", "Home"); }
            //// ...
            if (articleId==null)
                {
                // 如果articleId未提供，您可以採取適當的措施，例如返回錯誤信息或默認值。
                // 預設頁面articleId=20;
                articleId = 20;
                 }
            else 
                { 
                articleId = articleId.Value; 
                 }
                
                // 使用articleId進行相應的處理
                var comments = _context.TComments
                    .Include(e => e.FMember)
                    .Where(e => e.FArticleId == articleId.Value)
                    .ToList();

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

            if (dto.FArticleId == null)
            {
                // 如果articleId未提供，您可以採取適當的措施，例如返回錯誤信息或默認值。
                // 預設頁面articleId=20;
                dto.FArticleId = 20;
            }
            else
            {
                dto.FArticleId = dto.FArticleId;
            }

            //dto.FArticleId = 20;
            //dto.Content = "test123";

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
  

        public IActionResult Edit(int? id)
        {

            TComment comment = _context.TComments.FirstOrDefault(p => p.FCommentId == id);
            if (comment != null)
                return RedirectToAction("List");
            return View();
        }
        [HttpPost]
        public IActionResult Edit(TComment pIN)
        {

            TComment pDB = _context.TComments.FirstOrDefault(p => p.FCommentId == pIN.FCommentId);
            if (pDB != null)
            {
                pDB.FArticleId = pIN.FArticleId;
                pDB.FCommentContent = pIN.FCommentContent;
                pDB.FMemberId = pIN.FMemberId;
                pDB.FCommentTime = DateTime.Now;
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }



        public IActionResult Delete(int? id, int? articleId)
        {
            TComment comment = _context.TComments.FirstOrDefault(p => p.FCommentId == id);
            if (comment != null)
            {
                _context.TComments.Remove(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("List", new { articleId = articleId });
        }




        private bool TCommentExitsts(int id)
        {
            return (_context.TComments?.Any(e => e.FCommentId == id)).GetValueOrDefault();
        }


    }
}