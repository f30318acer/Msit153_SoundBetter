using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using prjMusicBetter.Models.ViewModels;
using NuGet.Protocol;

namespace Music_matchmaking_platform.Controllers
{
    public class PlaceController : Controller
    {
		private readonly dbSoundBetterContext _context;
		public PlaceController(dbSoundBetterContext context)
		{
			_context = context;
		}
		public IActionResult List()
		{
			var dbSoundBetterContext = _context.TSites.Include(t => t.FCity).Include(t => t.FMember);
			return Json(dbSoundBetterContext);
		}
		public IActionResult QueryByCity(int? id)//CityId
		{
			if (id == null || _context.TSites == null)
			{
				return NotFound();
			}

			var tSite = _context.TSites.Where(m => m.FCityId == id);
			if (tSite == null)
			{
				return NotFound();
			}
			return Json(tSite);
		}
		public IActionResult PlaceDetail()
        {
            return View();
        }
        public IActionResult Place()
        {
            return View();
        }
        public IActionResult BgPlaceManage()
        {
            return View();
        }
    }
}
