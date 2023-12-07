﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using System.Formats.Asn1;

namespace prjMusicBetter.Controllers
{
	public class apiTClassController : Controller
	{
		private readonly IWebHostEnvironment _host;
		private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        public apiTClassController(IWebHostEnvironment host, dbSoundBetterContext context, UserInfoService userInfoService)
		{
			_host = host;
			_context = context;
            _userInfoService = userInfoService;//抓使用者
        }

		//===全部課程的資料===
		public IActionResult List()
		{
			var dbSoundBetterContext = from s in _context.TClasses
									   join c in  _context.TClassClicks
									   on s.FClassId equals c.FClassId
                                       join m in _context.TMembers
                                       on s.FTeacherId equals m.FMemberId
                                       select new
                                       {
                                           fClassId = s.FClassId,
                                           fClassName = s.FClassName,
                                           fThumbnailPath = s.FThumbnailPath,
                                           fSkillId = s.FSkillId,
                                           fDescription = s.FDescription,
                                           fOnLine = s.FOnLine,
                                           fClick = c.FClick,
                                           fTeacherNmae = m.FName,
                                       };
            TMember member = _userInfoService.GetMemberInfo();
            var classfav = _context.TClassFavs.Where(m => m.FMemberId == member.FMemberId).Select(t => t.FClassId);
            ViewBag.classfav = classfav;//我喜歡哪些課
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

        public IActionResult classFav(int? id)
        {
            var classfav = _context.TClassFavs.Where(m => m.FClassId == id).Select(t => t.FMemberId);
            return Json(classfav);//這堂課有誰喜歡
        }

        public IActionResult FavQueryById()
        {
            TMember member = _userInfoService.GetMemberInfo();
            var classfav = _context.TClassFavs.Where(m => m.FMemberId == member.FMemberId);
            return Json(classfav);//我喜歡哪些課
        }

        // 新增我的最愛
        [HttpPost]
        public async Task<IActionResult> CreateFav(int classId)
        {
            TMember member = _userInfoService.GetMemberInfo();
            var memberId = member.FMemberId;
            var tClassFav = new TClassFav { FClassId = classId, FMemberId = memberId };

            _context.TClassFavs.Add(tClassFav);
            await _context.SaveChangesAsync();

            return Ok();//如果返回Ok()，就表示不向客户端返回任何信息，只告诉客户端请求成功
        }

        // 刪除我的最愛
        [HttpPost]
        public async Task<IActionResult> DeleteFav(int classId)
        {
            TMember member = _userInfoService.GetMemberInfo();
            var memberId = member.FMemberId;
            var tClassFav = await _context.TClassFavs.FirstOrDefaultAsync(f => f.FClassId == classId && f.FMemberId == memberId);

            if (tClassFav != null)
            {
                _context.TClassFavs.Remove(tClassFav);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
