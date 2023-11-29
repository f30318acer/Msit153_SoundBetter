using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using WebApplication1.Models;

namespace prjMusicBetter.Controllers
{
    public class apiTArticleController : Controller
    {
        private readonly dbSoundBetterContext _context;
        public apiTArticleController(dbSoundBetterContext context)
        {
            _context = context;
        }
        //===List_All===
        public IActionResult List()
        {
            var dbSoundBetterContext = _context.TArticles;
            return Json(dbSoundBetterContext);
        }
        //===List_MemberID===
        public IActionResult QueryByMember(int? id)//MemberId
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = _context.TArticles.Where(m => m.FMemberId == id);
            if (tArticle == null)
            {
                return NotFound();
            }
            return Json(tArticle);
        }
        //===搜尋===
        public IActionResult QueryById(int? id)
        {
            if (id == null || _context.TArticles == null)
            {
                return NotFound();
            }

            var tArticle = _context.TArticles.FirstOrDefault(m => m.FArticleId == id);
            if (tArticle == null)
            {
                return NotFound();
            }
            return Json(tArticle);
        }
        //===新增===
        [HttpPost]
        public IActionResult Create([FromBody] TArticle? article)
        {
            if (article != null)
            {
                _context.Add(article);
                _context.SaveChanges();
                return Content("新增成功");
            }
            return Content("錯誤");
        }

    }
}
