using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using SendGrid.Helpers.Mail;//
using SendGrid;//
using System;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using Microsoft.SqlServer.Server;
using System.Threading.Tasks;//

namespace Music_matchmaking_platform.Controllers
{
    public class PlaceController : Controller
    {
		private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly UserInfoService _userInfoService;
        public PlaceController(IWebHostEnvironment host, dbSoundBetterContext context, UserInfoService userInfoService)
		{
            _host = host;
            _context = context;
            _userInfoService = userInfoService;//抓使用者
        }
        public IActionResult Index()
        {
            return View();
        }

        Dictionary<int, string> cityIdMapping = new Dictionary<int, string>
        {
            {1, "tw-cl"},
            {2, "tw-tw"},
            {3, "tw-tp"},
            {4, "tw-ty"},
            {5, "tw-hs"},
            {6, "tw-hh"},
            {7, "tw-ml"},
            {8, "tw-th"},
            {9, "tw-cg"},
            {10, "tw-nt"},
            {11, "tw-yl"},
            {12, "tw-cs"},
            {13, "tw-ch"},
            {14, "tw-tn"},
            {15, "tw-kh"},
            {16, "tw-pt"},
            {17, "tw-tt"},
            {18, "tw-hl"},
            {19, "tw-il"},
            {20, "tw-ph"},
            {21, "tw-km"},
            {22, "tw-lk"}
        };
        public IActionResult CountSiteByCity()
        {
            var dbSoundBetterContext =
                from cityId in cityIdMapping.Keys
                join site in _context.TSites on cityId equals site.FCityId into joinedData
                from siteGroup in joinedData.DefaultIfEmpty()
                group siteGroup by cityId into groupedData
                select new
                {
                    fCityId = cityIdMapping.ContainsKey(groupedData.Key) ? cityIdMapping[groupedData.Key] : "tw-tw",
                    Count = groupedData.Count(item => item != null)
                };

            var result = dbSoundBetterContext.ToArray();

            return Json(result);
        }
        public IActionResult List()
		{
			var dbSoundBetterContext = _context.TSites
				.Include(t => t.FCity)
				.Include(t => t.FMember)
				.Select(t => new
				{
					fSiteId = t.FSiteId,
					fSiteName = t.FSiteName,
					fPhone = t.FPhone,
					fAddress = t.FAddress,
					fCity = t.FCity.FCity,
					fSiteType = t.FSiteType,
					fName = t.FMember.FName,
					fPicturePath = t.FPicture,
					fCityId = t.FCityId
				});

			return Json(dbSoundBetterContext);
		}
		public IActionResult GetCities()
		{
			var cities = _context.TCities;
			return Json(cities);
		}
		public IActionResult QueryByCity(int? fCityId)
		{
            var query = _context.TSites
				.Include(t => t.FCity)
				.Include(t => t.FMember)
				.Where(t => t.FCityId == fCityId)
				.Select(t => new
				{
					fSiteId = t.FSiteId,
					fSiteName = t.FSiteName,
					fPhone = t.FPhone,
					fAddress = t.FAddress,
					fCity = t.FCity.FCity,
					fSiteType = t.FSiteType,
					fName = t.FMember.FName,
					fPicturePath = t.FPicture,
					fCityId = t.FCityId
				});

			return Json(query);
        }
        public IActionResult Place()
        {
            return View();
        }
        public IActionResult BgPlaceManage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit(TSitePeriod? pIn)
        {
            TSitePeriod pDb = _context.TSitePeriods.FirstOrDefault(p => p.FSiteId == pIn.FSiteId);
          
            if (pDb != null && pIn != null)
            {
                pDb.FMonMorning = pIn.FMonMorning;
                pDb.FMonAfternoon = pIn.FMonAfternoon;
                pDb.FMonNight = pIn.FMonNight;
                pDb.FMonMidnight = pIn.FMonMidnight;
                pDb.FTuesMorning = pIn.FTuesMorning;
                pDb.FTuesAfternoon = pIn.FTuesAfternoon;
                pDb.FTuesNight = pIn.FTuesNight;
                pDb.FTuesMidnight = pIn.FTuesMidnight;
                pDb.FWedMorning = pIn.FWedMorning;
                pDb.FWedAfternoon = pIn.FWedAfternoon;
                pDb.FWedNight = pIn.FWedNight;
                pDb.FWedMidnight = pIn.FWedMidnight;
                pDb.FThurMorning = pIn.FThurMorning;
                pDb.FThurAfternoon = pIn.FThurAfternoon;
                pDb.FThurNight = pIn.FThurNight;
                pDb.FThurMidnight = pIn.FThurMidnight;
                pDb.FFriMorning = pIn.FFriMorning;
                pDb.FFriAfternoon = pIn.FFriAfternoon;
                pDb.FFriNight = pIn.FFriNight;
                pDb.FFriMidnight = pIn.FFriMidnight;
                pDb.FSatMorning = pIn.FSatMorning;
                pDb.FSatAfternoon = pIn.FSatAfternoon;
                pDb.FSatNight = pIn.FSatNight;
                pDb.FSatMidnight = pIn.FSatMidnight;
                pDb.FSunMorning = pIn.FSunMorning;
                pDb.FSunAfternoon = pIn.FSunAfternoon;
                pDb.FSunNight = pIn.FSunNight;
                pDb.FSunMidnight = pIn.FSunMidnight;

                _context.SaveChanges();
                return Content("場地預約成功");
            }
            return Content("錯誤");
        }
        public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.TSites == null)
			{
				return NotFound();
			}

			var tSite = await _context.TSites
				.Include(t => t.FCity)
				.Include(t => t.FMember)
				.Include(t => t.TSitePeriods)
				.FirstOrDefaultAsync(t => t.FSiteId == id);

			if (tSite == null)
			{
				return NotFound();
			}

			return View(tSite);
		}
        public IActionResult GetMemberNameandEmail()
        {
            TMember member = _userInfoService.GetMemberInfo();
            var name = member?.FName;
            var email = member?.FEmail;
            var result = new
            {
                Name = name,
                Email = email
            };

            return Json(result);
        }
        public IActionResult GetClasses(int? fSiteId)
        {
            var sss = from s in _context.TSites
                      join c in _context.TClasses 
                      on s.FSiteId equals c.FSiteId 
                      where s.FSiteId == fSiteId
                      select c;

            return Json(sss);
        }
        [HttpPost]
        public IActionResult sendEmail(EmailVM? formData2)
        {
            try
            {
                // 發送 Email
                SendReservationConfirmationEmail(formData2.Email, formData2.Name, formData2.Subject, formData2.Message, formData2.ReserName);

                return Ok(); // 或其他適合的回應
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // 適當地處理錯誤
            }
        }
        public void SendReservationConfirmationEmail(string Email, string Name, string Subject, string Message, string ReserName)
        {
            string apiKey = "SG.lHmKulaIQviZJqGMxJASPw.cfzz大家好SXXTSASxSUZKidD0AzgEGqaVsxpJm3tW5qv5ga0"; // 替換為你的 SendGrid API Key

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("zackyandjacky@gmail.com", "SoundBetter"); // 替換為實際的寄件者 Email 和名稱
            var to = new EmailAddress(Email, Name);
            var plainTextContent = $"親愛的 {Name}，\n\n{Subject}。以下是預約詳情：\n\n預約會員：{ReserName}\n訊息：{Message}";
            var htmlContent = $"<p>親愛的 {Name}，</p><p>{Subject}。以下是預約詳情：</p><p><strong>預約會員：</strong>{ReserName}</p><p><strong>訊息：</strong>{Message}</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, Subject, plainTextContent, htmlContent);

             client.SendEmailAsync(msg);
        }
    }
}
