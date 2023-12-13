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
using prjMusicBetter.Models.infra;
using WebApplication1.Models;

namespace prjMusicBetter.Controllers
{
    public class apiTArticleController : Controller
    {
        private readonly UserInfoService _userInfoService;
        private IWebHostEnvironment _enviro = null;
        private readonly dbSoundBetterContext _context;
        public apiTArticleController(IWebHostEnvironment p,dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _enviro = p;
            _userInfoService = userInfoService;
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
        public IActionResult Create(TArticle? Article, IFormFile formFilePhoto)
        {
            if (Article != null)
            {
                if (formFilePhoto != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    Article.FPhotoPath = photoName;
                    formFilePhoto.CopyTo(new FileStream(_enviro.WebRootPath + "/img/Article/" + photoName, FileMode.Create));
                }
                else
                {
                    Article.FPhotoPath = "1.jpg";
                }
               
                DateTime now = DateTime.Now;
                Article.FUpdateTime = now;
                _context.Add(Article);
                _context.SaveChanges();

                // 新增 TClassClicks 資料




                return Content("新增成功");
            }
            return Content("錯誤");
        }
        public IActionResult ArticleFav(int? id)
        {
            var Articlefav = _context.TArticleFavs.Where(m => m.FArticleId == id).Select(t => t.FMemberId);
            return Json(Articlefav);//這堂課有誰喜歡
        }
        public IActionResult FavQueryById()
        {
            TMember member = _userInfoService.GetMemberInfo();
            var Articlefav = _context.TArticleFavs.Where(m => m.FMemberId == member.FMemberId);
            return Json(Articlefav);//我喜歡哪些課
        }
        // 新增我的最愛
        [HttpPost]
        public async Task<IActionResult> CreateFav(int ArticleId)
        {
            TMember member = _userInfoService.GetMemberInfo();
            var memberId = member.FMemberId;
            var tArticleFav = new TArticleFav { FArticleId = ArticleId, FMemberId = memberId };

            _context.TArticleFavs.Add(tArticleFav);
            await _context.SaveChangesAsync();

            return Ok();//如果返回Ok()，就表示不向客户端返回任何信息，只告诉客户端请求成功
        }

        // 刪除我的最愛
        [HttpPost]
        public async Task<IActionResult> DeleteFav(int ArticleId)
        {
            TMember member = _userInfoService.GetMemberInfo();
            var memberId = member.FMemberId;
            var tArticleFav = await _context.TArticleFavs.FirstOrDefaultAsync(f => f.FArticleId == ArticleId && f.FMemberId == memberId);

            if (tArticleFav != null)
            {
                _context.TArticleFavs.Remove(tArticleFav);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        //===PicturesByID===
        public IActionResult QueryPictureByID(int? id)//MemberId
		{
			if (id == null || _context.TArticlePictures == null)
			{
				return NotFound();
			}

			var pictures = _context.TArticlePictures.Where(m => m.FArticleId == id);
			if (pictures == null)
			{
				return NotFound();
			}
			return Json(pictures);
		}
        //===搜尋ArticlePic用ID===
        public IActionResult QueryPicById(int? id)
        {
            if (id == null || _context.TArticlePictures == null)
            {
                return NotFound();
            }

            var pic = _context.TArticlePictures.Where(m => m.FArticleId == id);
            if (pic == null)
            {
                return NotFound();
            }
            return Json(pic);
        }
    }
}
