using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;

namespace prjSoundBetterApi.Controllers
{
    public class apiTWorkController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private IWebHostEnvironment _enviro = null;
        private readonly UserInfoService _userInfoService;
        public apiTWorkController(IWebHostEnvironment p,dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _enviro = p;
            _userInfoService = userInfoService;
        }
        //===List_All===
        public IActionResult List()
        {
            var dbSoundBetterContext = _context.TWorks;
            return Json(dbSoundBetterContext);
        }

        public IActionResult MemberList()
        {
            var x = from m in _context.TMembers
                    join w in _context.TWorks on m.FMemberId equals w.FMemberId
                   
                    select new {w.FWorkId,m.FMemberId ,m.FUsername,w.FFilePath,w.FWorkName ,m.FPhotoPath};
            var y = x.Take(3);
            return Json(y); 
        }
        public IActionResult ListWithUserName()
        {
            var dbSoundBetterContext = from w in _context.TWorks
                                       join m in _context.TMembers
                                       on w.FMemberId equals m.FMemberId
                                       select new { m.FUsername, w.FDescription, w.FFilePath, w.FStyleId, w.FThumbnail,w.FWorkName, w.FWorkId };

            return Json(dbSoundBetterContext);
        }
        //===List_MemberID===
        public IActionResult QueryByMember(int? id)//MemberId
        {
            if (id == null || _context.TWorks == null)
            {
                return NotFound();
            }

            var tProject = _context.TWorks.Where(m => m.FMemberId == id);
            if (tProject == null)
            {
                return NotFound();
            }
            return Json(tProject);
        }
        //===新增===
        [HttpPost]
        //public IActionResult Create([FromBody] TWork? work)
        //{
        //    if (work != null)
        //    {
        //        _context.Add(work);
        //        _context.SaveChanges();
        //        TWorkClick WorkClick = new TWorkClick
        //        {
        //            FWorkId = work.FWorkId, // 使用 TClass 資料的 FClassId
        //            FClick = 0
        //        };

        //        _context.Add(WorkClick);
        //        _context.SaveChanges();
        //        return Content("新增成功");
        //    }
        //    return Content("錯誤");

        //}
        //===新增===
        [HttpPost]
        public IActionResult Create(TWork? work, IFormFile formFilePhoto, IFormFile formFileDemo)
        {
            if (work != null)
            {
                if (formFilePhoto != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    work.FThumbnail = photoName;
                    formFilePhoto.CopyTo(new FileStream(_enviro.WebRootPath + "/img/Works/" + photoName, FileMode.Create));
                }
                else {
                    work.FThumbnail = "1.jpg";
                }
                if (formFileDemo != null)
                {
                    string DemoName = Guid.NewGuid().ToString() + ".mp3";
                    work.FFilePath = DemoName;
                    formFileDemo.CopyTo(new FileStream(_enviro.WebRootPath + "/WorkFile/" + DemoName, FileMode.Create));
                }
                DateTime now = DateTime.Now;
                work.FUpdateTime= now;
                _context.Add(work);
                _context.SaveChanges();

                // 新增 TClassClicks 資料
                TWorkClick  workClick = new TWorkClick
                {
                    FWorkId = work.FWorkId, // 使用 TClass 資料的 FClassId
                    FClick = 0
                };

                _context.Add(workClick);
                _context.SaveChanges();
                return Content("新增成功");
            }
            return Content("錯誤");
        }
        private bool TWorkExists(int id)
        {
            return (_context.TWorks?.Any(e => e.FWorkId == id)).GetValueOrDefault();
        }

        public IActionResult Delete(int id)
        {
            if (_context.TWorks == null)
            {
                return Problem("連線錯誤");
            }
            var project = _context.TWorks.Where(c => c.FWorkId == id).FirstOrDefault();
            if (project != null)
            {
                // 刪除 TClassClick 中符合條件的資料
                var relatedClicks = _context.TWorkClicks.Where(click => click.FWorkId == project.FWorkId).ToList();
                _context.TWorkClicks.RemoveRange(relatedClicks);
                _context.TWorks.Remove(project);
                _context.SaveChanges();
                return Content("刪除成功");
            }
            return Content("刪除失敗");
        }
    }
}
