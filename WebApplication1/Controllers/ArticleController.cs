﻿using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.Dtos.Comment;
using prjMusicBetter.Models.infra;
using System.Linq.Expressions;
using System.Security.Claims;

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

            //var tArticle = await _context.TArticles
            //    .Include(t => t.FMember)
            //    .Include(t => t.FStyle)
            //    .FirstOrDefaultAsync(m => m.FArticleId == id);

            var tArticle = await _context.TArticles
              .Include(t => t.FMember)
              .Include(t => t.FStyle)
              .Include(c => c.TComments)
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


            //留言部分
            //await _context.TArticles.Include(c => c.TComments).FirstOrDefaultAsync(m => m.FArticleId == id);
            //留言部分
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



        //mdto.FMemberID = _userInfoService.GetMemberInfo().FMemberId;
        //cdto.FArticleId= _context.TArticles.SingleOrDefault().FArticleId;

        ///留言功能
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int id, CommentDto cdto)
        {

            cdto.FMemberId = _userInfoService.GetMemberInfo().FMemberId;
            cdto.FArticleId = _context.TArticles.Include(t => t.FMember).Include(t => t.FStyle).Where(e => e.FArticleId == id).SingleOrDefault().FArticleId;


            _context.TComments.Add(new TComment()
            {   
                FCommentId = cdto.FCommentId,
                FMemberId = cdto.FMemberId,
                FCommentContent = cdto.FCommentContent,
                FArticleId = cdto.FArticleId,
                FCommentTime = DateTime.Now,
            });
            _context.SaveChanges();

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", id);
            //return RedirectToAction("Details","Article", cdto.FArticleId);

        }


        //public IActionResult GetMemberNameandEmail()
        //{

        //    var name = member?.FName;
        //    var email = member?.FEmail;
        //    var result = new
        //    {
        //        Name = name,
        //        Email = email
        //    };

        //    return Json(result);
        //}
    }
}
