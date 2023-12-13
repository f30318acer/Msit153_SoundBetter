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
            var dbSoundBetterContext = _context.TProjects.Where(p => p.FProjectStatusId == 1);
            return Json(dbSoundBetterContext);
        }
		public IActionResult SkillsList()
		{
			var dbSoundBetterContext = _context.TSkills;
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
        //===專案資訊===
        public IActionResult PrjSkillStyleInfo(int? id)
        {
            if (id != null)
            {
                TProject prj = _context.TProjects.FirstOrDefault(p => p.FProjectId == id);
                if (prj != null)
                {
                    string skillName = _context.TSkills.FirstOrDefault(s => s.FSkillId == prj.FSkillId).FName;
                    string styleName = _context.TStyles.FirstOrDefault(s => s.FStyleId == prj.FStyleId).FName;
                    string prjStatus = _context.TProjectStatuses.FirstOrDefault(s => s.FProjectStatusId == prj.FProjectStatusId).FDescription;
                    return Content($"專案風格 : {styleName} / 需求技能 : {skillName} / 專案狀態 : [{prjStatus}]");
                }               
            }
            return NotFound();
        }
		//===AppliNum_By_ProjectID===
		public IActionResult QueryApppliNumById(int? id)//ProjectId
		{
			if (id == null || _context.TProjects == null)
			{
				return Content("0");
			}

			var appliRec = _context.TApplicationRecords.Where(m => m.FProjectId == id);
			if (appliRec == null)
			{
				return Content("0");
			}
			return Content(appliRec.Count().ToString());
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
        public IActionResult Create(TProject? project,IFormFile formFilePhoto, IFormFile formFileDemo)
        {            
            if (project != null)
            {
                if (formFilePhoto != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    project.FThumbnailPath = photoName;
                    formFilePhoto.CopyTo(new FileStream(_enviro.WebRootPath + "/img/project/" + photoName, FileMode.Create));
                }
                if (formFileDemo != null)
                {
                    string DemoName = Guid.NewGuid().ToString() + ".mp3";
                    project.FDemoFilePath = DemoName;
                    formFileDemo.CopyTo(new FileStream(_enviro.WebRootPath + "/ProjectDemo/" + DemoName, FileMode.Create));
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
                    string DemoName = pDb.FDemoFilePath;
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

            var project = _context.TProjects.FirstOrDefault(c => c.FProjectId == id);

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
        //===新增應徵===
        [HttpPost]
        public IActionResult AppliProject(int id, TApplicationRecord appli)
        {
            if (appli != null)
            {
                DateTime now = DateTime.Now;
                appli.FProjectId = id;
                appli.FApplicationStatusId = 1;
                _context.Add(appli);
                _context.SaveChanges();
                return Content("新增成功");
            }
            return Content("錯誤");
        }
        //===新增追蹤===
        [HttpPost]
        public IActionResult favProject(int id, TProjectFav fav)
        {
            if (fav != null)
            {
                DateTime now = DateTime.Now;
                fav.FProjectId = id;
                _context.Add(fav);
                _context.SaveChanges();
                return Content("追蹤成功");
            }
            return Content("錯誤");
        }
        //===取消追蹤===
        [HttpPost]
        public IActionResult DisFavProject(TProjectFav fav)
        {
            TProjectFav fDb = _context.TProjectFavs.FirstOrDefault(f => f.FMemberId == fav.FMemberId && f.FProjectId == fav.FProjectId);
            if (fDb != null)
            {
                _context.Remove(fDb);
                _context.SaveChanges();
                return Content("取消追蹤");
            }
            return Content("錯誤");
        }

        //===取得應徵資料===
        public IActionResult GetAppliInfo(int? id)
        {
            if (id == null || _context.TApplicationRecords == null)
            {
                return NotFound();
            }
            var appliInfo = from a in _context.TApplicationRecords
                            join m in _context.TMembers on a.FMemberId equals m.FMemberId
                            join s in _context.TApplicationStatuses on a.FApplicationStatusId equals s.FApplicationStatus
                            where a.FProjectId == id
                            select new
                            {
                                a.FApplicationRecordId,
                                a.FMemberId,
                                a.FApplicationStatusId,
                                s.FDescription,
                                a.FSelfIntroduction,
                                m.FUsername,
                                m.FName,
                                m.FEmail,
                                m.FPhone
                            };
            return Json(appliInfo);
        }
        //===改變應徵紀錄檢視狀態===
        [HttpPost]
        public IActionResult ChangeAppliStatus(int? id)
        {
            if (id != null)
            {
                var record = _context.TApplicationRecords.FirstOrDefault(a => a.FApplicationRecordId == id);
                record.FApplicationStatusId = 2;
                _context.SaveChanges();
                return Content("檢視成功");
            }
            return Content("錯誤");
        }

    }
}
