using Microsoft.AspNetCore.Mvc;

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
					fPicturePath = t.FPicture
				})
				.ToList();
			return Json(dbSoundBetterContext);
		}
		public IActionResult GetCities()
		{
			var cities = _context.TCities.ToList();
			return Json(cities);
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
