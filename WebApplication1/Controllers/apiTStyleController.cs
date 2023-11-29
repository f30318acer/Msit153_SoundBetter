using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace prjSoundBetterApi.Controllers
{
    public class apiTStyleController : Controller
    {
        private readonly dbSoundBetterContext _context;
        public apiTStyleController(dbSoundBetterContext context)
        {
            _context = context;
        }
        //===List_All===
        public IActionResult List()
        {
            var dbSoundBetterContext = _context.TStyles;
            return Json(dbSoundBetterContext);
        }
    }
}
