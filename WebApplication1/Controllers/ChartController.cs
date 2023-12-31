﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using prjMusicBetter.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;
using prjMusicBetter.Models.ViewModels;


namespace prjMusicBetter.Controllers
{
    public class ChartController : Controller
    {

        private readonly dbSoundBetterContext _context;
        public ChartController(dbSoundBetterContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult clock()
        {
            return View();
        }
        public IActionResult MemberIndex()
        {
            return View();
        }
        public IActionResult PowerBI()
        {
            return View();
        }
        public IActionResult DealClass()
        {
            return View();
        }
        public IActionResult ProjectSkills()
        {
            return View();
        }
        public IActionResult DealProject()
        {
            return View();
        }
        public IActionResult classclick()
        {
            return View();
        }
        public IActionResult workclick()
        {
            return View();
        }
        public IActionResult projectclick()
        {
            return View();
        }
        public IActionResult siteclick()
        {
            return View();
        }
        public IActionResult application()
        {
            return View();
        }
        public IActionResult CombinedClicks()
        {
            return View("CombinedClicks");

        }

        public IActionResult GenderRatio()
        {

            var genderCount = _context.TMembers
                .GroupBy(M => M.FGender)
                .Select(group => new
                {
                    Gender = group.Key ? "Male" : "Female",
                    Count = group.Count()
                })
                .ToList();
            return Json(genderCount);
        }


        [HttpPost]
        public IActionResult GetClassClicksData()
        {
            // 首先，從 TClasses 表中選擇所有類別名稱和ID
            var classInfo = _context.TClasses
                .Select(c => new { c.FClassId, c.FClassName })
                .ToList();

            // 接著，從 TClassClick 表中選擇類別ID和對應的點擊次數
            var clickInfo = _context.TClassClicks
                .GroupBy(cc => cc.FClassId)
                .Select(g => new { FClassId = g.Key, FClick = g.Sum(x => x.FClick) })
                .ToList();

            // 然後，將這兩個列表聯接在一起，以 FClassId 為聯接條件
            var classClicksData = classInfo
                .Join(clickInfo,
                    c => c.FClassId,
                    cc => cc.FClassId,
                    (c, cc) => new
                    {
                        c.FClassName,
                        cc.FClick
                    })
                .ToList();

            // 最後，將結果轉換為您需要的格式，這裡假設您需要兩個分開的列表
            List<object> data = new List<object>
            {
                classClicksData.Select(x => x.FClassName).ToList(),
                classClicksData.Select(x => x.FClick).ToList()
            };

            return Json(data); // 將數據包裝成 JSON 格式返回
        }


        [HttpPost]
        public IActionResult GetWorksClicksData()
        {
            //List<object> data = new List<object>();

            //List<string> works = _context.TWorks.Select(w => w.FWorkName).ToList();

            //data.Add(works);

            //var ClicksWorks = _context.TWorks.Select(c => c.FClick).ToList();
            //data.Add(ClicksWorks);
            //return data;

            // 首先，從 TClasses 表中選擇所有類別名稱和ID
            var workInfo = _context.TWorks
                .Select(c => new { c.FWorkId, c.FWorkName })
                .ToList();

            // 接著，從 TClassClick 表中選擇類別ID和對應的點擊次數
            var clickInfo = _context.TWorks
                .GroupBy(cc => cc.FWorkId)
                .Select(g => new { FWorkId = g.Key, FClick = g.Sum(x => x.FClick) })
                .ToList();

            // 然後，將這兩個列表聯接在一起，以 FClassId 為聯接條件
            var workClicksData = workInfo
                .Join(clickInfo,
                    c => c.FWorkId,
                    cc => cc.FWorkId,
                    (c, cc) => new
                    {
                        c.FWorkName,
                        cc.FClick
                    })
                .ToList();

            // 最後，將結果轉換為您需要的格式，這裡假設您需要兩個分開的列表
            List<object> data = new List<object>
            {
                workClicksData.Select(x => x.FWorkName).ToList(),
                workClicksData.Select(x => x.FClick).ToList()
            };

            return Json(data); // 將數據包裝成 JSON 格式返回
        }
        [HttpPost]
        public IActionResult GetProjectStatusData()
        {
            // 首先，從 TClasses 表中選擇所有類別名稱和ID
            var ProjectStatusInfo = _context.TProjectStatuses
                .Select(c => new { c.FProjectStatusId, c.FDescription })
                .ToList();

            // 接著，從 TClassClick 表中選擇類別ID和對應的點擊次數
            var ProjectIDInfo = _context.TProjects
                .GroupBy(cc => cc.FMemberId)
                .Select(g => new { FProjectStatusId = g.Key, FMemberId = g.Sum(x => x.FMemberId) })
                .ToList();

            // 然後，將這兩個列表聯接在一起，以 FClassId 為聯接條件
            var ProjectStatusData = ProjectStatusInfo
                .Join(ProjectIDInfo,
                    c => c.FProjectStatusId,
                    cc => cc.FProjectStatusId,
                    (c, cc) => new
                    {
                        c.FDescription,
                        cc.FMemberId
                    })
                .ToList();

            // 最後，將結果轉換為您需要的格式，這裡假設您需要兩個分開的列表
            List<object> data = new List<object>
            {
                ProjectStatusData.Select(x => x.FDescription).ToList(),
                ProjectStatusData.Select(x => x.FMemberId).ToList()
            };

            return Json(data); // 將數據包裝成 JSON 格式返回


        }
        [HttpPost]
        public IActionResult GetSitesStatusData()
        {
            // 首先，從 TClasses 表中選擇所有類別名稱和ID
            var ProjectStatusInfo = _context.TSitePeriodStatuses
                .Select(c => new { c.FSitePeriodStatusId, c.FDescription })
                .ToList();

            // 接著，從 TClassClick 表中選擇類別ID和對應的點擊次數
            var ProjectIDInfo = _context.TSites
                .GroupBy(cc => cc.FSiteId)
                .Select(g => new { FSitePeriodStatusId = g.Key, FSiteId = g.Sum(x => x.FSiteId) })
                .ToList();

            // 然後，將這兩個列表聯接在一起，以 FClassId 為聯接條件
            var ProjectStatusData = ProjectStatusInfo
                .Join(ProjectIDInfo,
                    c => c.FSitePeriodStatusId,
                    cc => cc.FSitePeriodStatusId,
                    (c, cc) => new
                    {
                        c.FDescription,
                        cc.FSiteId
                    })
                .ToList();

            // 最後，將結果轉換為您需要的格式，這裡假設您需要兩個分開的列表
            List<object> data = new List<object>
            {
                ProjectStatusData.Select(x => x.FDescription).ToList(),
                ProjectStatusData.Select(x => x.FSiteId).ToList()
            };

            return Json(data); // 將數據包裝成 JSON 格式返回


        }
        [HttpPost]
        public IActionResult GetStatusCounts()
        {
            // 獲取應用狀態和對應的描述
            var applicationStatus = _context.TApplicationRecords
                .GroupBy(ar => ar.FApplicationStatusId)
                .Select(group => new
                {
                    StatusId = group.Key,
                    Description = _context.TApplicationStatuses
                        .FirstOrDefault(ps => ps.FApplicationStatus == group.Key).FDescription,
                    Count = group.Count()
                })
                .ToList();
            // 合併應用狀態和項目狀態
            var combinedStatus = applicationStatus
                .GroupBy(cs => cs.Description)
                .Select(group => new
                {
                    Description = group.Key,
                    TotalCount = group.Sum(g => g.Count)
                })
                .ToList();

            // 返回給前端的數據結構
            var result = combinedStatus.Select(cs => new { label = cs.Description, value = cs.TotalCount }).ToList();

            return Json(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetMonthlyDealData()
        {
            var monthlyData = await _context.TDealClasses
                .GroupBy(d => new
                {
                    Year = d.FDealdate.Year,
                    Month = d.FDealdate.Month
                })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    TotalPrice = group.Sum(d => d.FPrice)
                })
                .OrderBy(result => result.Year)
                .ThenBy(result => result.Month)
                .ToListAsync();

            return Json(monthlyData);
        }
        public async Task<IActionResult> GetProjectSkills()
        {
            var projectSkills = await _context.TProjects
                .Join(
                    _context.TSkills,
                    project => project.FSkillId,
                    skill => skill.FSkillId,
                    (project, skill) => new { ProjectName = project.FName, SkillName = skill.FName }
                )
                .ToListAsync();

            return Json(projectSkills);
        }



    }
}
