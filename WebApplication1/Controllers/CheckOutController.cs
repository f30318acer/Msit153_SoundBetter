using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;

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
                    Quanity=2,
                    ImagePath="img/Image1.jpg"
                },
                 new ProductEntity
                {
                    Product = "TimeWear",
                    Rate=1000,
                    Quanity=1,
                    ImagePath="img/Image2.jpg"
                },
            };
            return View(productList);
        }
    }
}
