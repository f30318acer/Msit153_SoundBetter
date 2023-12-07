using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace prjMusicBetter.Controllers
{
    public class EmailController : Controller
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public EmailController(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }
        [HttpGet]
        public IActionResult SendResetPasswordEmail()
        {
            return View();
        }  
    }
}
