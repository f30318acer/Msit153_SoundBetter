using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using System.Formats.Asn1;

namespace prjMusicBetter.Controllers
{
	public class apiTClassController : Controller
	{
		private readonly IWebHostEnvironment _host;
		private readonly dbSoundBetterContext _context;
		public apiTClassController(IWebHostEnvironment host, dbSoundBetterContext context)
		{
			_host = host;
			_context = context;
		}

		//===全部課程的資料===
		public IActionResult List()
		{
			var dbSoundBetterContext = _context.TClasses;
			return Json(dbSoundBetterContext);
		}
		//===用MemberID搜尋===
		public IActionResult QueryByMember(int? id)//MemberId
		{
			if (id == null || _context.TClasses == null)
			{
				return NotFound();
			}

			var tProject = _context.TClasses.Where(m => m.FTeacherId == id);
			if (tProject == null)
			{
				return NotFound();
			}
			return Json(tProject);
		}
		//===List_Status===
		public IActionResult QueryBySkillID(int? id)
		{
			if (id == null || _context.TClasses == null)
			{
				return NotFound();
			}

			var tProject = _context.TClasses.Where(m => m.FSkillId == id);
			if (tProject == null)
			{
				return NotFound();
			}
			return Json(tProject);
		}
		//===搜尋id===
		public IActionResult QueryById(int? id)
		{
			if (id == null || _context.TClasses == null)
			{
				return NotFound();
			}

			var tProject = _context.TClasses.FirstOrDefault(m => m.FClassId == id);
			if (tProject == null)
			{
				return NotFound();
            }

            /*var SiteId = _context.TSites.Where(t => t.FSiteId == tProject.FSiteId).Select(t => t.FSiteName).SingleOrDefault();
            ViewBag.FSiteId = SiteId;//地點名稱

            var TName = _context.TMembers.Where(t => t.FMemberId == tProject.FTeacherId).Select(t => t.FName).SingleOrDefault();
            ViewBag.TeacherName = TName;//教師名稱

            var TPhotoPath = _context.TMembers.Where(t => t.FMemberId == tProject.FTeacherId).Select(t => t.FPhotoPath).SingleOrDefault();
            ViewBag.TeacherPhoto = TPhotoPath;//教師照片

            var Introduction = _context.TMembers.Where(t => t.FMemberId == tProject.FTeacherId).Select(t => t.FIntroduction).SingleOrDefault();
            ViewBag.teacher = Introduction;//教師自述

            if (id <= (_context.TClasses.Count() - 2))
            {
                var Preclass = _context.TClasses.Where(t => t.FClassId == (id + 1)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.PrClass = Preclass;//下一個課程名稱
                var Senclass = _context.TClasses.Where(t => t.FClassId == (id + 2)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.SenClass = Senclass;//下下一個課程名稱
            }
            else
            {
                var Preclass = _context.TClasses.Where(t => t.FClassId == (id - 1)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.PrClass = Preclass;//上一個課程名稱
                var Senclass = _context.TClasses.Where(t => t.FClassId == (id - 2)).Select(t => t.FClassName).SingleOrDefault();
                ViewBag.SenClass = Senclass;//上上一個課程名稱
            }
            ViewBag.AllClass = _context.TClasses.Count();//有多少課程


            ViewBag.Id = id;//id*/
            return Json(tProject);
		}
		//===新增===
		[HttpPost]
		public IActionResult Create(TClass? project, IFormFile formFile)
		{
			if (project != null)
			{
                if (formFile != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    project.FThumbnailPath = photoName;
                    formFile.CopyTo(new FileStream(_host.WebRootPath + "/img/classimg/" + photoName, FileMode.Create));
                }
				else
				{
                    project.FThumbnailPath = "class_bg.jpg";
                }

                
                _context.Add(project);
				_context.SaveChanges();
				return Content("新增成功");
			}
			return Content("錯誤");
		}
		//===修改===
		public IActionResult Edit(int id, TClass project, IFormFile formFile)
		{
			if (project != null)
			{
				try
				{
					//圖片有改就存下並修改
                    if (formFile != null)
                    {
                        string photoName = _context.TClasses.Where(m => m.FClassId == project.FClassId).Select(t => t.FThumbnailPath).SingleOrDefault();
                        project.FThumbnailPath = photoName;
                        formFile.CopyTo(new FileStream(_host.WebRootPath + "/img/classimg/" + photoName, FileMode.Create));
                    }
                    //圖片沒改就沿用
                    else
                    {
						string Path = _context.TClasses.Where(m => m.FClassId == project.FClassId).Select(t => t.FThumbnailPath).SingleOrDefault();
						project.FThumbnailPath = Path;
                    }
                    _context.Update(project);
					_context.SaveChanges();
					return Content("修改成功");
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TProjectExists(project.FClassId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}
			return Content("錯誤");
		}
		//===刪除===
		public IActionResult Delete(int id)
		{
			if (_context.TClasses == null)
			{
				return Problem("連線錯誤");
			}
			var project = _context.TClasses.Where(c => c.FClassId == id).FirstOrDefault();
			if (project != null)
			{
				_context.TClasses.Remove(project);
				_context.SaveChanges();
				return Content("刪除成功");
			}
			return Content("刪除失敗");
		}
		private bool TProjectExists(int id)
		{
			return (_context.TClasses?.Any(e => e.FClassId == id)).GetValueOrDefault();
		}
	}
}
