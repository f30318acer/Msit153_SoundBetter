using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace prjMusicBetter.Controllers
{
    public class apiTCommentController : Controller
    {
        private readonly UserInfoService _userInfoService;
        private readonly dbSoundBetterContext _context;
        public apiTCommentController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;
        }
        //===List_All===
        public IActionResult List()
        {
            var dbSoundBetterContext = _context.TComments;
            return Json(dbSoundBetterContext);
        }
        //===List_MemberID===
        public IActionResult QueryByMember(int? id)//MemberId
        {
            if (id == null || _context.TComments == null)
            {
                return NotFound();
            }

            var tComment = _context.TComments.Where(m => m.FMemberID == id);
            if (tComment == null)
            {
                return NotFound();
            }
            return Json(tComment);
        }
        //===搜尋===
        public IActionResult QueryById(int? id)
        {
            if (id == null || _context.TComments == null)
            {
                return NotFound();
            }

            var tComment = _context.TComments.FirstOrDefault(m => m.FCommentID == id);
            if (tComment == null)
            {
                return NotFound();
            }
            return Json(tComment);
        }
        //===新增===
        [HttpPost]
        public IActionResult Create([FromBody] TComment? comment)
        {
            if (comment != null)
            {
                _context.Add(comment);
                _context.SaveChanges();
                return Content("新增成功");
            }
            return Content("錯誤");
        }

        //public IActionResult CommentFav(int? id)
        //{
        //    var Commentfav = _context.TCommentFav.Where(m => m.TCommentID == id).Select(t => t.FMemberId);
        //    return Json(CommentFav);//留言+讚
        //}
        
    }
}