﻿using Microsoft.AspNetCore.Mvc;

namespace prjMusicBetter.Controllers
{
	public class MembersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Memberclass()
		{
			return View();
		}
	}
}
