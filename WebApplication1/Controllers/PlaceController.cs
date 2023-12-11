using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using Microsoft.SqlServer.Server;
using System.Threading.Tasks;

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
        //public IActionResult GetMemberNameandEmail()
        //{
        //    TMember member = _userInfoService.GetMemberInfo();
        //    var name = member?.FName;
        //    var email = member?.FEmail;
        //    var result = new
        //    {
        //        Name = name,
        //        Email = email
        //    };

        //    return Json(result);
        //}
        [HttpPost]
        public IActionResult sendEmail(EmailVM? formData2)
        {
            try
            {
                // 發送 Email
                SendReservationConfirmationEmail(formData2.Email, formData2.Name, formData2.Subject, formData2.Message);

                return Ok(); // 或其他適合的回應
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // 適當地處理錯誤
            }
        }
        public void SendReservationConfirmationEmail(string Email, string Name, string Subject, string Message)
        {
            string apiKey = "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"; // 替換為你的 SendGrid API Key

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jonson50815@yahoo.com.tw", "陳品諺"); // 替換為實際的寄件者 Email 和名稱
            var to = new EmailAddress(Email, Name);
            var plainTextContent = $"親愛的 {Name}，\n\n感謝您的預約。以下是您的預約詳情：\n\n主旨：{Subject}\n訊息：{Message}";
            var htmlContent = $"<p>親愛的 {Name}，</p><p>感謝您的預約。以下是您的預約詳情：</p><p><strong>主旨：</strong>{Subject}</p><p><strong>訊息：</strong>{Message}</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, Subject, plainTextContent, htmlContent);

             client.SendEmailAsync(msg);
        }
    }
}
