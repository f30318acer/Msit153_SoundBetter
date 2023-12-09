using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Models;
using prjMusicBetter.Models.ViewModels;
using prjMusicBetter.Helpers;
using prjMusicBetter.Models.infra;

namespace prjMusicBetter.Controllers
{
    public class apiShoppingCartController : Controller
    {
        // Constructor
        private readonly UserInfoService _userInfoService;
        public apiShoppingCartController(UserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 取得購物車Session
        /// </summary>
        public IActionResult Index()
        {   
            int memberId = _userInfoService.GetMemberId();

            var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();
            return Json(cart);
        }

        /// <summary>
        /// 將商品加入購物車
        /// </summary>
        public IActionResult AddClassToCart([FromBody] ShoppingCartVM request)
        {
            try
            {
                // 空的話錯誤
                if (request.ProductId == 0)
                {
                    return Json(new { success = false, message = $"加入購物車失敗 : 請聯絡客服" });
                }

                int memberId = _userInfoService.GetMemberId();

                // 從 Session 中取得購物車資料，如果不存在則初始化一個空的 List
                var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();

                // 檢查購物車中是否已經有相同的課程
                var existingProduct = cart.FirstOrDefault(item => item.ProductId == request.ProductId);
                
                if (existingProduct != null)
                {
                    // 如果購物車中已經有相同的課程，將其數量 +1
                    existingProduct.ProjectCount += 1;
                }
                else
                {
                    // 如果購物車中沒有該課程，將其添加到購物車
                    ShoppingCartVM shoppingCartVM = new ShoppingCartVM();
                    shoppingCartVM.ProductId = request.ProductId;
                    shoppingCartVM.ProductName = request.ProductName;
                    shoppingCartVM.ProjectPrice = request.ProjectPrice;
                    shoppingCartVM.ProductDesc = request.ProductDesc;
                    shoppingCartVM.ProjectCount = 1;

                    cart.Add(shoppingCartVM);
                }


                // 將商品加入購物車
                HttpContext.Session.Set($"ShoppingCart_{memberId}", cart);

                return Json(new { success = true, message = "加入成功" , data = cart});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"加入購物車失敗 : 請聯絡客服" });
            }
        }

        /// <summary>
        /// 更新購物車中的物品數量
        /// </summary>
        public IActionResult UpdateCartItem(int productId, int productCount)
        {
            int memberId = _userInfoService.GetMemberId();

            var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();

            // 找到要更新的物品
            var productToUpdate = cart.Find(item => item.ProductId == productId);
            if (productToUpdate != null)
            {
                productToUpdate.ProjectCount = productCount;

                // 數量0 = 刪除
                if (productCount == 0)
                {
                    cart.Remove(productToUpdate);
                }
            }

            HttpContext.Session.Set($"ShoppingCart_{memberId}", cart);
            return Json(new { success = true, message = "更新成功" });
        }


    }
}