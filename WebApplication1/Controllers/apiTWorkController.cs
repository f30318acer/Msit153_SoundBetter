using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjSoundBetterApi.Models;

namespace prjSoundBetterApi.Controllers
{
    public class apiTWorkController : Controller
    {
        private readonly dbSoundBetterContext _context;
        public apiTWorkController(dbSoundBetterContext context)
        {
            _context = context;
        }
        //===List_All===
        public IActionResult List()
        {
            var dbSoundBetterContext = _context.TWorks;
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
