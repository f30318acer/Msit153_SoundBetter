using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;
using Stripe.Checkout;


namespace prjMusicBetter.Controllers
{
    public class CheckOutController : Controller
    {
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
            return View("Login");

        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult CheckOut()
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
                  new ProductEntity
                {
                    Product = "Timex",
                    Rate=300,
                    Quantity=2,
                    ImagePath="img/Image2.jpg"
                }
            };

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
    }
}
