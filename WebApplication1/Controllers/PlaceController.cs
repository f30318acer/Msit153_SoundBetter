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
        public IActionResult Edit(int id, TSitePeriod? pIn)
        {
            TSitePeriod pDb = _context.TSitePeriods.FirstOrDefault(p => p.FSiteId == id);

            if (pDb != null && pIn != null)
            {
                pDb.FMonMorning = pIn.FMonMorning.Value;
                pDb.FMonAfternoon = pIn.FMonAfternoon.Value;
                pDb.FMonNight = pIn.FMonNight.Value;
                pDb.FMonMidnight = pIn.FMonMidnight.Value;
                pDb.FTuesMorning = pIn.FTuesMorning.Value;
                pDb.FTuesAfternoon = pIn.FTuesAfternoon.Value;
                pDb.FTuesNight = pIn.FTuesNight.Value;
                pDb.FTuesMidnight = pIn.FTuesMidnight.Value;
                pDb.FWedMorning = pIn.FWedMorning.Value;
                pDb.FWedAfternoon = pIn.FWedAfternoon.Value;
                pDb.FWedNight = pIn.FWedNight.Value;
                pDb.FWedMidnight = pIn.FWedMidnight.Value;
                pDb.FThurMorning = pIn.FThurMorning.Value;
                pDb.FThurAfternoon = pIn.FThurAfternoon.Value;
                pDb.FThurNight = pIn.FThurNight.Value;
                pDb.FThurMidnight = pIn.FThurMidnight.Value;
                pDb.FFriMorning = pIn.FFriMorning.Value;
                pDb.FFriAfternoon = pIn.FFriAfternoon.Value;
                pDb.FFriNight = pIn.FFriNight.Value;
                pDb.FFriMidnight = pIn.FFriMidnight.Value;
                pDb.FSatMorning = pIn.FSatMorning.Value;
                pDb.FSatAfternoon = pIn.FSatAfternoon.Value;
                pDb.FSatNight = pIn.FSatNight.Value;
                pDb.FSatMidnight = pIn.FSatMidnight.Value;
                pDb.FSunMorning = pIn.FSunMorning.Value;
                pDb.FSunAfternoon = pIn.FSunAfternoon.Value;
                pDb.FSunNight = pIn.FSunNight.Value;
                pDb.FSunMidnight = pIn.FSunMidnight.Value;

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
