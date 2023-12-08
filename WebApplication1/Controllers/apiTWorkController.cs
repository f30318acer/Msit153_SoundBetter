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
        private readonly UserInfoService _userInfoService;
        public apiTWorkController(dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
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
                    join w in _context.TWorks
                    on m.FMemberId equals w.FMemberId
                    select new {m.FMemberId ,m.FUsername,w.FFilePath,w.FWorkName ,m.FPhotoPath};
            var y = x.Take(3);
            return Json(y); 
        }
        public IActionResult ListWithUserName()
        {
            var dbSoundBetterContext = from w in _context.TWorks
                                       join m in _context.TMembers
                                       on w.FMemberId equals m.FMemberId
                                       select new { m.FUsername, w.FDescription, w.FFilePath, w.FStyle, w.FThumbnail,w.FWorkName };

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
        public IActionResult Create([FromBody] TWork? work)
        {
            if (work != null)
            {
                _context.Add(work);
                _context.SaveChanges();
                return Content("新增成功");
            }
            return Content("錯誤");
        }
        private bool TWorkExists(int id)
        {
            return (_context.TWorks?.Any(e => e.FWorkId == id)).GetValueOrDefault();
        }
    }
}
