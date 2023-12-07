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
    public class apiTVisionController : Controller
    {
        //private readonly UserInfoService _userInfoService;
        //private readonly dbSoundBetterContext _context;
        //public apiTVisionController(dbSoundBetterContext context, UserInfoService userInfoService)
        //{
        //    _context = context;
        //    _userInfoService = userInfoService;
        //}
        ////===List_All===
        //public IActionResult List()
        //{
        //    var dbSoundBetterContext = _context.TVision;
        //    return Json(dbSoundBetterContext);
        //}
        ////===List_MemberID===
        //public IActionResult QueryByMember(int? id)//MemberId
        //{
        //    if (id == null || _context.TVision == null)
        //    {
        //        return NotFound();
        //    }

        //    var tVision = _context.TVision.Where(m => m.FMemberId == id);
        //    if (tVision == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(tVision);
        //}
        ////===搜尋===
        //public IActionResult QueryById(int? id)
        //{
        //    if (id == null || _context.TVision == null)
        //    {
        //        return NotFound();
        //    }

        //    var tVision = _context.TVision.FirstOrDefault(m => m.FVisionId == id);
        //    if (tVision == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(tVision);
        //}
        ////===新增===
        //[HttpPost]
        //public IActionResult Create([FromBody] TVision? vision)
        //{
        //    if (vision != null)
        //    {
        //        _context.Add(vision);
        //        _context.SaveChanges();
        //        return Content("新增成功");
        //    }
        //    return Content("錯誤");
        //}
        ////===PicturesByID===
        //public IActionResult QueryPictureByID(int? id)//MemberId
        //{
        //    if (id == null || _context.TVisionPictures == null)
        //    {
        //        return NotFound();
        //    }

        //    var v = _context.TVision.Where(m => m.FVisionId == id);
        //    if (v == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(v);
        //}
        ////===搜尋VisionPic用ID===
        //public IActionResult QueryPicById(int? id)
        //{
        //    if (id == null || _context.TVision == null)
        //    {
        //        return NotFound();
        //    }

        //    var pic = _context.TVisionPictures.Where(m => m.FVisionId == id);
        //    if (pic == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(pic);
        //}
    }
}
