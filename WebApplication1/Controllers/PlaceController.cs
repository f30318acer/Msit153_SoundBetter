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
        [HttpPost]
        public IActionResult Edit(TSitePeriod? pIn)
        {
            TSitePeriod pDb = _context.TSitePeriods.FirstOrDefault(p => p.FSiteId == pIn.FSiteId);
          
            if (pDb != null && pIn != null)
            {
                pDb.FMonMorning = pIn.FMonMorning;
                pDb.FMonAfternoon = pIn.FMonAfternoon;
                pDb.FMonNight = pIn.FMonNight;
                pDb.FMonMidnight = pIn.FMonMidnight;
                pDb.FTuesMorning = pIn.FTuesMorning;
                pDb.FTuesAfternoon = pIn.FTuesAfternoon;
                pDb.FTuesNight = pIn.FTuesNight;
                pDb.FTuesMidnight = pIn.FTuesMidnight;
                pDb.FWedMorning = pIn.FWedMorning;
                pDb.FWedAfternoon = pIn.FWedAfternoon;
                pDb.FWedNight = pIn.FWedNight;
                pDb.FWedMidnight = pIn.FWedMidnight;
                pDb.FThurMorning = pIn.FThurMorning;
                pDb.FThurAfternoon = pIn.FThurAfternoon;
                pDb.FThurNight = pIn.FThurNight;
                pDb.FThurMidnight = pIn.FThurMidnight;
                pDb.FFriMorning = pIn.FFriMorning;
                pDb.FFriAfternoon = pIn.FFriAfternoon;
                pDb.FFriNight = pIn.FFriNight;
                pDb.FFriMidnight = pIn.FFriMidnight;
                pDb.FSatMorning = pIn.FSatMorning;
                pDb.FSatAfternoon = pIn.FSatAfternoon;
                pDb.FSatNight = pIn.FSatNight;
                pDb.FSatMidnight = pIn.FSatMidnight;
                pDb.FSunMorning = pIn.FSunMorning;
                pDb.FSunAfternoon = pIn.FSunAfternoon;
                pDb.FSunNight = pIn.FSunNight;
                pDb.FSunMidnight = pIn.FSunMidnight;

                _context.SaveChanges();
                return Content("修改成功");
            }
            return Content("錯誤");
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
