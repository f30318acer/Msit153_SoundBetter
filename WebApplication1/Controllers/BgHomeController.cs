using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;
using prjMusicBetter.Models.ViewModels;
using prjMusicBetter.Models.Services;

namespace prjMusicBetter.Controllers
{
    public class BgHomeController : Controller
	{
		private readonly dbSoundBetterContext _context;
        private readonly MemberService _memberService;
		public BgHomeController(dbSoundBetterContext context ,MemberService memberService)
		{
			_context = context;
            _memberService = memberService;
		}

        public IActionResult Index()
		{
			return View();
		}	 
    }
}
