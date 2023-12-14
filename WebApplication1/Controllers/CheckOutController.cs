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
        private readonly UserInfoService _userInfoService;
        private readonly dbSoundBetterContext _context;

        public CheckOutController(UserInfoService userInfoService, dbSoundBetterContext dbSoundBetterContext)
        {
            _userInfoService = userInfoService;
            _context = dbSoundBetterContext;
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
        public IActionResult CheckOut(List<OrderVM> orderItems, int totalPrice)
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            foreach (var item in orderItems)
            {
                productList.Add(new ProductEntity
                {
                    Product = item.ProductName,
                    Rate = item.ProductPrice,
                    Quantity = item.ProductCount,
                    ImagePath = "img/Image1.jpg" // You might want to update this path based on your actual requirements
                });
            }

            var domain = "http://localhost:7078/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",//付款完出現
                CancelUrl = domain + "CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = "f50318acer@gmail.com"

            };
            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(item.Rate * item.Quantity),
                        Currency = "inr",
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

        public IActionResult Checkout2()
        {
            // 取得Session
            int memberId = _userInfoService.GetMemberId();

            var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();
            cart = checkShoppingCart(cart);

            // 可能缺貨 => 回傳此問題，讓使用者重新確認
            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(List));
            }

            // 成功才跳轉
            return View(cart);
        }

        public List<ShoppingCartVM> checkShoppingCart(List<ShoppingCartVM> cart)
        {
            foreach (ShoppingCartVM item in cart)
            {
                // 購物車內Class資料更新，檢查待補
                var classData = _context.TClasses.FirstOrDefault(m => m.FClassId == item.ProductId);

                if (classData != null)
                {

                    item.ProductName = classData.FClassName;
                    item.ProductPrice = classData.FPrice;
                    item.ProductDesc = classData.FDescription;
                    item.ProductThumbnailPath = classData.FThumbnailPath;
                    item.ProductStartDate = classData.FStartdate;
                    item.ProductEndDate = classData.FEnddate;

                    // 如果出現售完、已刪除等等
                    if (classData.FCurrentStudent >= classData.FMaxStudent)
                    {
                        item.ProductStatus = 9999;
                    }

                    // 取得場地名稱
                    item.SiteName = _context.TSites.Where(t => t.FSiteId == classData.FSiteId).Select(t => t.FSiteName).SingleOrDefault();

                    // 取得教師名稱
                    item.TeacherName = _context.TMembers.Where(t => t.FMemberId == classData.FTeacherId).Select(t => t.FName).SingleOrDefault();

                    item.ProductStatus = 0000;
                }
            }
            return cart;
        }
        public IActionResult List()
        {
            // 取得Session
            int memberId = _userInfoService.GetMemberId();
            var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();

            cart = checkShoppingCart(cart);

            return View(cart);
        }
    }
}
