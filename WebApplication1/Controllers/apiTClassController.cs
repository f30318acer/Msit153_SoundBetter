using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using SendGrid.Helpers.Mail;
using System.Formats.Asn1;
using System.Text.RegularExpressions;

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
        //===去除資料的html標籤===
        public static string ReplaceHtmlTag(string Html)
        {
            Html = Regex.Replace(Html, "<[^>]*>", "");
            return Html;
        }
        //===全部課程的資料===
        public IActionResult List()
        {
            var dbSoundBetterContext = from s in _context.TClasses
                                       join c in _context.TClassClicks
                                       on s.FClassId equals c.FClassId
                                       join m in _context.TMembers
                                       on s.FTeacherId equals m.FMemberId
                                       join t in _context.TSites
                                       on s.FSiteId equals t.FSiteId

                                       where s.FEnddate >= DateTime.Now
                                       orderby s.FClassId descending
                                       select new
                                       {
                                           fClassId = s.FClassId,
                                           fClassName = s.FClassName,
                                           fPrice = s.FPrice,
                                           fThumbnailPath = s.FThumbnailPath,
                                           fCurrentStudent = s.FCurrentStudent,
                                           fMaxStudent = s.FMaxStudent,
                                           fOnLine = s.FOnLine,
                                           fSkillId = s.FSkillId,
                                           fDescription = ReplaceHtmlTag(s.FDescription),
                                           fClick = c.FClick,
                                           fTeacherNmae = m.FUsername,
                                           fSiteName = t.FSiteName,
                                       };

            return Json(dbSoundBetterContext);
        }
        //===用MemberID搜尋===
        public IActionResult QueryByMember(int? id)//MemberId
        {
            if (id == null || _context.TClasses == null)
            {
                return NotFound();
            }

            var dbSoundBetterContext = from s in _context.TClasses
                                       join c in _context.TClassClicks
                                       on s.FClassId equals c.FClassId
                                       join m in _context.TMembers
                                       on s.FTeacherId equals m.FMemberId
                                       where s.FTeacherId == id
                                       orderby s.FClassId descending
                                       select new
                                       {
                                           fClassId = s.FClassId,
                                           fClassName = s.FClassName,
                                           fThumbnailPath = s.FThumbnailPath,
                                           fCurrentStudent = s.FCurrentStudent,
                                           fMaxStudent = s.FMaxStudent,
                                           fSkillId = s.FSkillId,
                                           fDescription = ReplaceHtmlTag(s.FDescription),
                                           fOnLine = s.FOnLine,
                                           fClick = c.FClick,
                                           fTeacherNmae = m.FUsername,
                                           fEnddate = s.FEnddate,
                                       };
            if (dbSoundBetterContext == null)
            {
                return NotFound();
            }
            return Json(dbSoundBetterContext);
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


                // 新增 TClass 資料
                _context.Add(project);
                _context.SaveChanges();

                // 新增 TClassClicks 資料
                TClassClick classClick = new TClassClick
                {
                    FClassId = project.FClassId, // 使用 TClass 資料的 FClassId
                    FClick = 0
                };

                _context.Add(classClick);
                _context.SaveChanges();

                // 取得 TSites 的相關資料
                var siteData = _context.TSites.SingleOrDefault(s => s.FSiteId == project.FSiteId);

                // 假設 TSites 中有相對應的資料
                if (siteData == null)
                {
                    // 整理資料
                    TSite newSiteData = new TSite
                    {
                        // 設定屬性值，可根據 TSites 的屬性進行調整
                        FSiteName = siteData.FSiteName,
                        FMemberId = siteData.FMemberId,
                        FSiteType = siteData.FSiteType,
                        FCityId = siteData.FCityId,
                        FAddress = siteData.FAddress,
                    };

                    // 新增到 _context.TSites
                    _context.TSites.Add(newSiteData);

                    // 提交更改以確保新站點數據被保存
                    _context.SaveChanges();

                    // 更新 project 的 FSiteId
                    project.FSiteId = newSiteData.FSiteId;

                    // 再次保存對 project 的更改
                    _context.SaveChanges();
                }

                return Content("新增成功");
            }
            return Content("錯誤");
        }
        //===修改===
        public IActionResult Edit(TClass project, IFormFile formFile)
        {
            if (project != null)
            {
                try
                {
                    //圖片有改就存下並修改
                    if (formFile != null)
                    {
                        string photoName = _context.TClasses.Where(m => m.FClassId == project.FClassId).Select(t => t.FThumbnailPath).SingleOrDefault();
                        if (photoName != "class_bg.jpg")
                        {
                            project.FThumbnailPath = photoName;
                            formFile.CopyTo(new FileStream(_host.WebRootPath + "/img/classimg/" + photoName, FileMode.Create));
                        }
                        photoName = Guid.NewGuid().ToString() + ".jpg";
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
                // 刪除 TClassClick 中符合條件的資料
                var relatedClicks = _context.TClassClicks.Where(click => click.FClassId == project.FClassId).ToList();
                _context.TClassClicks.RemoveRange(relatedClicks);

                // 刪除 TDealClassDetails 中符合條件的資料
                var relatedDeals = _context.TDealClassDetails.Where(click => click.FClassId == project.FClassId).ToList();
                _context.TDealClassDetails.RemoveRange(relatedDeals);

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


        //我的最愛
        public IActionResult FavQueryById()
        {
            TMember member = _userInfoService.GetMemberInfo();
            var classfav = _context.TClassFavs.Where(m => m.FMemberId == member.FMemberId);
            if (classfav.Count() > 0)
            {
                return Json(classfav);
            }
            return NotFound();
        }
        public IActionResult FavById(int id)
        {
            var result = from s in _context.TClasses
                         join f in _context.TClassFavs on s.FClassId equals f.FClassId
                         join c in _context.TClassClicks on s.FClassId equals c.FClassId
                         join m in _context.TMembers on s.FTeacherId equals m.FMemberId
                         where f.FMemberId == id
                         orderby s.FClassId descending
                         select new
                         {
                             fClassId = s.FClassId,
                             fClassName = s.FClassName,
                             fThumbnailPath = s.FThumbnailPath,
                             fCurrentStudent = s.FCurrentStudent,
                             fMaxStudent = s.FMaxStudent,
                             fSkillId = s.FSkillId,
                             fDescription = ReplaceHtmlTag(s.FDescription),
                             fOnLine = s.FOnLine,
                             fClick = c.FClick,
                             fTeacherNmae = m.FUsername,
                             fEnddate = s.FEnddate,
                         };
            return Json(result.ToList());
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

        //我買的課程
        public IActionResult AddCurrentStudent(int id)
        {
            var result = from s in _context.TClasses
                         join f in _context.TDealClassDetails on s.FClassId equals f.FClassId
                         join c in _context.TClassClicks on s.FClassId equals c.FClassId
                         join m in _context.TMembers on s.FTeacherId equals m.FMemberId
                         where f.FMemberId == id
                         orderby s.FClassId descending
                         select new
                         {
                             fClassId = s.FClassId,
                             fClassName = s.FClassName,
                             fThumbnailPath = s.FThumbnailPath,
                             fCurrentStudent = s.FCurrentStudent,
                             fMaxStudent = s.FMaxStudent,
                             fSkillId = s.FSkillId,
                             fDescription = ReplaceHtmlTag(s.FDescription),
                             fOnLine = s.FOnLine,
                             fClick = c.FClick,
                             fTeacherNmae = m.FUsername,
                             fEnddate = s.FEnddate,
                         };
            return Json(result.ToList());
        }

        //退出課程
        [HttpPost]
        public IActionResult DelDeal(int? id, int? classId)
        {
            if (id == null || classId == null)
            {
                return BadRequest("參數不正確");
            }

            try
            {
                // 根據你的需求，進行 TDealClassDetails 的刪除操作
                var dealToRemove = _context.TDealClassDetails.FirstOrDefault(d => d.FClassId == classId && d.FMemberId == id);
                var classCurrent = _context.TClasses.FirstOrDefault(c => c.FClassId == classId);
                if (dealToRemove != null || classCurrent != null)
                {
                    --classCurrent.FCurrentStudent;

                    _context.TDealClassDetails.Remove(dealToRemove);
                    _context.SaveChanges();
                    // 刪除成功的處理，這裡可以根據需要進行其他操作
                    return Ok("成功刪除");
                }
                else
                {
                    // 找不到對應的資料
                    return NotFound("找不到對應的資料");
                }
            }
            catch (Exception ex)
            {
                // 處理例外情況
                return StatusCode(500, "刪除失敗");
            }
        }

        //新增地點
        [HttpPost]
        public IActionResult CreateSite(TSite? tSite)
        {
            if (tSite != null)
            {
                tSite.FPicture = "NoImage.jpg";
                _context.Add(tSite);
                _context.SaveChanges();
                return Json(new { fSiteId = tSite.FSiteId, fSiteName = tSite.FSiteName });
            }
            return Content("錯誤");
        }

        //完成結帳後的處理
        //fCurrentStudent + 1
        [HttpPost]
        public IActionResult Currentplus(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                TMember member = _userInfoService.GetMemberInfo();
                var memberId = member.FMemberId;
                var Class = _context.TClasses.FirstOrDefault(m => m.FClassId == id);
                if (Class != null)
                {
                    Class.FCurrentStudent++;//現在學生數+1

                    var studentID = Class.FTeacherId;
                    var studentName = _context.TMembers.FirstOrDefault(m => m.FMemberId == memberId).FUsername;
                    var ClassName = Class.FClassName;
                    var Notifi = "有一名學生：" + studentName + "，加入了你的課程：" + ClassName;

                    var tClassNotifi = new TNotification
                    {
                        FMemberId = studentID,
                        FNotification = Notifi,
                        FNotifiStatus = 1,
                        FProjectId = 0,
                        FClassId = id
                    };
                    _context.TNotifications.Add(tClassNotifi);
                    _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // 在這裡處理異常情況，可以記錄異常，並返回相應的 HTTP 響應
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
