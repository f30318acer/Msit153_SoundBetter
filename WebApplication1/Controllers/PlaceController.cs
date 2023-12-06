using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using System;

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
			var dbSoundBetterContext = _context.TSites
				.Include(t => t.FCity)
				.Include(t => t.FMember)
				.Select(t => new
				{
					fSiteId = t.FSiteId,
					fSiteName = t.FSiteName,
					fPhone = t.FPhone,
					fAddress = t.FAddress,
					fCity = t.FCity.FCity,
					fSiteType = t.FSiteType,
					fName = t.FMember.FName,
					fPicturePath = t.FPicture,
					fCityId = t.FCityId
				});

			return Json(dbSoundBetterContext);
		}
		public IActionResult GetCities()
		{
			var cities = _context.TCities;
			return Json(cities);
		}
		public IActionResult QueryByCity(int? fCityId)
		{
            var query = _context.TSites
				.Include(t => t.FCity)
				.Include(t => t.FMember)
				.Where(t => t.FCityId == fCityId)
				.Select(t => new
				{
					fSiteId = t.FSiteId,
					fSiteName = t.FSiteName,
					fPhone = t.FPhone,
					fAddress = t.FAddress,
					fCity = t.FCity.FCity,
					fSiteType = t.FSiteType,
					fName = t.FMember.FName,
					fPicturePath = t.FPicture,
					fCityId = t.FCityId
				});

			return Json(query);
        }
        public IActionResult Place()
        {
            return View();
        }
        public IActionResult BgPlaceManage()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.TSites == null)
			{
				return NotFound();
			}

			var tSite = await _context.TSites
				.Include(t => t.FCity)
				.Include(t => t.FMember)
				.Include(t => t.TSitePeriods)
				.FirstOrDefaultAsync(t => t.FSiteId == id);

			if (tSite == null)
			{
				return NotFound();
			}

			return View(tSite);
		}
	}
}
