using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using WebApplication1.Models;

namespace prjSoundBetterApi.Controllers
{
    public class apiTProjectsController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private IWebHostEnvironment _enviro = null;
        private readonly UserInfoService _userInfoService;
        public apiTProjectsController(IWebHostEnvironment p, dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _context = context;
            _enviro = p;
            _userInfoService = userInfoService;
        }
        //===List_All===
        public IActionResult List()
        {
            var dbSoundBetterContext = _context.TProjects;
            return Json(dbSoundBetterContext);
        }
        //===List_MemberID===
        public IActionResult QueryByMember(int? id)//MemberId
        {
            if (id == null || _context.TProjects == null)
            {
                return NotFound();
            }

            var tProject = _context.TProjects.Where(m => m.FMemberId == id);
            if (tProject == null)
            {
                return NotFound();
            }
            return Json(tProject);
        }
        //===List_Status===
        public IActionResult QueryByStatus(int? id)//StatusId
        {
            if (id == null || _context.TProjects == null)
            {
                return NotFound();
            }

            var tProject = _context.TProjects.Where(m => m.FProjectStatusId == id);
            if (tProject == null)
            {
                return NotFound();
            }
            return Json(tProject);
        }
        //===搜尋===
        public IActionResult QueryById(int? id)
        {
            if (id == null || _context.TProjects == null)
            {
                return NotFound();
            }

            var tProject = _context.TProjects.FirstOrDefault(m => m.FProjectId == id);
            if (tProject == null)
            {
                return NotFound();
            }
            return Json(tProject);
        }
        //===找業主===
		public IActionResult QueryMemberById(int? id)
		{
			if (id == null || _context.TMembers == null)
			{
				return NotFound();
			}

			var member = _context.TMembers.FirstOrDefault(m => m.FMemberId == id);
			if (member == null)
			{
				return NotFound();
			}
			return Json(member);
		}
		//===新增===
		[HttpPost]
        public IActionResult Create(TProject? project,IFormFile formFile)
        {            
            if (project != null)
            {
                if (formFile != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    project.FThumbnailPath = photoName;
                    formFile.CopyTo(new FileStream(_enviro.WebRootPath + "/img/project/" + photoName, FileMode.Create));
                }
                DateTime now = DateTime.Now;
                project.FStartdate = now;
                project.FProjectStatusId = 1;
                project.FSiteId = 1;
                _context.Add(project);
                _context.SaveChanges();
                return Content("新增成功");
            }
            return Content("錯誤");
        }
        //===修改===
        [HttpPost]
        public IActionResult Edit(int id, TProject? pIn, IFormFile? formFilePhoto,IFormFile? formFileDemo)
        {
            TProject pDb = _context.TProjects.FirstOrDefault(p => p.FProjectId == id);

            if (pDb != null && pIn != null)
            {
                pDb.FName = pIn.FName;
                pDb.FBudget = pIn.FBudget;
                pDb.FEnddate = pIn.FEnddate;
                pDb.FStyleId = pIn.FStyleId;
                pDb.FDescription = pIn.FDescription;
                pDb.FDescription2 = pIn.FDescription2;
                if (formFilePhoto != null)
                {
                    string photoName = pDb.FThumbnailPath;
                    formFilePhoto.CopyTo(new FileStream(_enviro.WebRootPath + "/img/project/" + photoName, FileMode.Create));
                }
                if (formFileDemo != null)
                {
                    string DemoName = "Demo_"+ pDb.FProjectId + ".mp3";
                    pDb.FDemoFilePath = DemoName;
                    formFileDemo.CopyTo(new FileStream(_enviro.WebRootPath + "/ProjectDemo/" + DemoName, FileMode.Create));
                }
                _context.SaveChanges();
                return Content("修改成功");
            }
            return Content("錯誤");
        }
        //===刪除===
        public IActionResult Delete(int id)
        {
            if (_context.TProjects == null)
            {
                return Problem("連線錯誤");
            }
            var project = _context.TProjects.Where(c => c.FProjectId == id).FirstOrDefault();
            if (project != null)
            {
                _context.TProjects.Remove(project);
                _context.SaveChanges();
                return Content("刪除成功");
            }
            return Content("刪除失敗");
        }
        private bool TProjectExists(int id)
        {
            return (_context.TProjects?.Any(e => e.FProjectId == id)).GetValueOrDefault();
        }
    }
}
