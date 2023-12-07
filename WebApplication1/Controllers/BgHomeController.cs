using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;
using prjMusicBetter.Models.ViewModels;


namespace prjMusicBetter.Controllers
{
    public class BgHomeController : Controller
	{
		private readonly dbSoundBetterContext _context;
       
		public BgHomeController(dbSoundBetterContext context)
		{
			_context = context;
        
		}

        public IActionResult Index()
		{
			return View();
		}	 
    }
}
