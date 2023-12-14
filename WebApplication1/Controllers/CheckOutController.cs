using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Helpers;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using Stripe.Checkout;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace prjMusicBetter.Controllers
{

    public class CheckOutController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly UserInfoService _userInfoService;
        public CheckOutController(IWebHostEnvironment host, dbSoundBetterContext context, UserInfoService userInfoService)
        {
            _host = host;
            _context = context;
            _userInfoService = userInfoService;//抓使用者
        }
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Tommy Hilfiger",
                    Rate=1500,
                    Quantity=2,
                    ImagePath="img/Image1.jpg"
                },
                 new ProductEntity
                {
                    Product = "TimeWear",
                    Rate=1000,
                    Quantity=1,
                    ImagePath="img/Image2.jpg"
                },
            };
            return View(productList);
        }


        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());


            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();

                return View("Success");
            }
            else
            {
                return View("Login");
            }


        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CheckOut(List<OrderVM> orderItems, int totalPrice)
        {
            TMember member = _userInfoService.GetMemberInfo();
            var email = member.FEmail;
            List<ProductEntity> productList = new List<ProductEntity>();
            foreach (var item in orderItems)
            {
                productList.Add(new ProductEntity
                {
                    Product = item.ProductName,
                    Rate = item.ProductPrice,
                    Quantity = item.ProductCount,
                    ImagePath = "img/Image1.jpg"
                });
            }

            var domain = "http://localhost:7078/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",//付款完出現
                CancelUrl = domain + "CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = $"{email}"

            };
            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(item.Rate * item.Quantity * 100),
                        Currency = "TWD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),

                        }
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

    }
}
