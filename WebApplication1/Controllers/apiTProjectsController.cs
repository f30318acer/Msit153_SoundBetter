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
                var allRec = _context.TApplicationRecords.Where(c => c.FProjectId == id);
                foreach (var rec in allRec) 
                {
                    rec.FApplicationStatusId = 3;
                }
                project.FProjectStatusId = 4;
                //_context.TProjects.Remove(project);
                _context.SaveChanges();
                return Content("取消成功");
            }

            return Content("取消失敗");
        }

        //===重啟===

        public IActionResult Revive(int id)
        {
            if (_context.TProjects == null)
            {
                return Problem("連線錯誤");
            }

            var project = _context.TProjects.FirstOrDefault(c => c.FProjectId == id);

            if (project != null)
            {
                project.FProjectStatusId = 1;
                //_context.TProjects.Remove(project);
                _context.SaveChanges();
                return Content("重啟成功");
            }

            return Content("重啟失敗");
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
                TApplicationRecord appliDb = _context.TApplicationRecords.Where(a => a.FProjectId == id).FirstOrDefault(a => a.FMemberId == appli.FMemberId);
                if (appliDb != null)
                {
                    var prj = _context.TProjects.FirstOrDefault(p => p.FProjectId == id);
                    appliDb.FSelfIntroduction = appli.FSelfIntroduction;
                    appliDb.FApplicationStatusId = 1;
                    var noti = _context.TNotifications.FirstOrDefault(a => a.FProjectId == appliDb.FProjectId && a.FMemberId == prj.FMemberId);
                    if (noti != null)
                    {
                        noti.FNotifiStatus = 1;
                    }
                    else 
                    {
                        //通知===
                        TNotification noti2 = new TNotification();                       
                        noti2.FNotifiStatus = 1;
                        noti2.FMemberId = prj.FMemberId;
                        noti2.FProjectId = id;
                        noti2.FClassId = 0;
                        noti2.FNotification = $"專案{prj.FProjectId}有新應徵";
                        _context.Add(noti2);
                        //======
                    }
                }
                else 
                {
                    DateTime now = DateTime.Now;
                    appli.FProjectId = id;
                    appli.FApplicationStatusId = 1;
                    _context.Add(appli);

                    //通知===
                    TNotification noti = new TNotification();
                    var prj = _context.TProjects.FirstOrDefault(p => p.FProjectId == appli.FProjectId);
                    noti.FNotifiStatus = 1;
                    noti.FMemberId = prj.FMemberId;
                    noti.FProjectId = id;
                    noti.FClassId = 0;
                    noti.FNotification = $"專案{prj.FProjectId}有新應徵";
                    _context.Add(noti);
                    //======
                }

                _context.SaveChanges();
                return Content("應徵成功");
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
        //===取得追蹤的專案===
        public IActionResult GetPrjFavByID(int? id)
        {
            if (id == null || _context.TProjectFavs == null)
            {
                return NotFound();
            }
            var prjFav = from f in _context.TProjectFavs
                         join p in _context.TProjects
                         on f.FProjectId equals p.FProjectId
                         where f.FMemberId == id
                         select new { f.FProjectId, p.FName, p.FDescription, p.FSkill };
			return Json(prjFav);
		}

		//===取得被錄取專案===

		public IActionResult GetPrjAcceptByID(int? id)
		{
			if (id == null || _context.TApplicationRecords == null)
			{
				return NotFound();
			}
			var prjAcc = from f in _context.TApplicationRecords
						 join p in _context.TProjects
						 on f.FProjectId equals p.FProjectId
						 where f.FMemberId == id && f.FApplicationStatusId == 4
						 select new { f.FProjectId, p.FName, p.FDescription, p.FSkill };
            if (prjAcc != null) 
            {
				return Json(prjAcc);
			}
			return NotFound();
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
        //===錄取===
        public IActionResult AcceptAppli(int? id)
        {
            if (id != null)
            {
                var record = _context.TApplicationRecords.FirstOrDefault(a => a.FApplicationRecordId == id);
                int prjID = record.FProjectId;
                var allrecord = _context.TApplicationRecords.Where(a => a.FProjectId ==  prjID);
                foreach (var item in allrecord)
                {
                    item.FApplicationStatusId = 3;
                }
                record.FApplicationStatusId = 4;
                TProject prj = _context.TProjects.FirstOrDefault(a => a.FProjectId == prjID);
                prj.FProjectStatusId = 2;
                //通知===
                var notiDb = _context.TNotifications.FirstOrDefault(n => n.FProjectId == prjID && n.FMemberId == record.FMemberId);
                if (notiDb != null)
                {
                    notiDb.FNotifiStatus = 1;
                }
                else 
                {
                    TNotification noti = new TNotification();
                    noti.FNotifiStatus = 1;
                    noti.FMemberId = record.FMemberId;
                    noti.FProjectId = prjID;
                    noti.FClassId = 0;
                    noti.FNotification = $"您被專案{prj.FProjectId}錄取了";
                    _context.Add(noti);
                }
                //======
                _context.SaveChanges();
                return Content("錄取成功");
            }
            return Content("錯誤");
        }
        //===取得錄取者資訊===
        public IActionResult GetAcceptMemberInfo(int? id)
        {
            if (id != null)
            {
                TApplicationRecord rec = _context.TApplicationRecords.Where(a => a.FProjectId == id).FirstOrDefault(a => a.FApplicationStatusId == 4);
                var member = from m in _context.TMembers
                                 where m.FMemberId == rec.FMemberId
                                 select new { 
                                 m.FMemberId,
                                 m.FUsername,
                                 m.FName,
                                 m.FPhone,
                                 m.FEmail,
                                 m.FIntroduction,
                                 m.FPhotoPath};
                return Json(member);
            }
            return Content("錯誤");
        }

    }
}
