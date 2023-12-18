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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var topMembers = _context.TWorks
    .OrderByDescending(w => w.FClick)
    .Take(3)
    .Join(
        _context.TMembers,
        work => work.FMemberId,
        member => member.FMemberId,
        (work, member) => new
        {
            work.FWorkId,
            member.FMemberId,
            member.FUsername,
            work.FFilePath,
            work.FWorkName,
            member.FPhotoPath
        }
    );

            return Json(topMembers);
        }
        public IActionResult ListWithUserName()
        {
            var worksWithUsername = from w in _context.TWorks
                                    join m in _context.TMembers on w.FMemberId equals m.FMemberId
                                    select new { m.FUsername, w.FDescription, w.FFilePath, w.FStyleId, w.FThumbnail, w.FWorkName, w.FWorkId, w.FClick };

            // 按 FClick 由多到少排序
            var sortedWorksWithUsername = worksWithUsername.OrderByDescending(w => w.FClick);

            return Json(sortedWorksWithUsername);
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
                work.FClick = 0;
                _context.Add(work);
                _context.SaveChanges();

                // 新增 TClassClicks 資料
              

               
              
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

        public IActionResult PlusOne(int?id)
        {
            var WorkClick = _context.TWorks.FirstOrDefault(c => c.FWorkId == id);
            if (WorkClick != null)
            {
                WorkClick.FClick++;//點閱數+1
            }
            _context.SaveChanges();
            return Content("新增成功");
        }
        public IActionResult AddToList(int? id)
        {
            if (id != null)
            {
                TMember member = _userInfoService.GetMemberInfo();
                int memberId = member.FMemberId;

                TPlaylist listDb = _context.TPlaylists.Where(p => p.FMemberId == memberId).FirstOrDefault(p => p.FWorkId == id);
                if (listDb == null)
                {
                    TPlaylist list = new TPlaylist();
                    list.FMemberId = memberId;
                    list.FWorkId = id;
                    list.FUpdateTime = DateTime.Now;
                    _context.Add(list);
                    _context.SaveChanges();
                    return Content("新增成功");
                }        
            }
            return NotFound();
        }
        public IActionResult GetPlayList() {
            TMember member = _userInfoService.GetMemberInfo();
            if (member != null) 
            {
                int memberId = member.FMemberId;
                var list = from p in _context.TPlaylists
                           join w in _context.TWorks on p.FWorkId equals w.FWorkId
                           where p.FMemberId == memberId orderby w.FUpdateTime
                           select new { w.FWorkId, w.FWorkName, w.FThumbnail,w.FFilePath };
               
                    return Json(list);
            }

            return NotFound();
        }
    }
}
