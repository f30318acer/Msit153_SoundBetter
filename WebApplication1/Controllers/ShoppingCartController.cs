using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Helpers;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserInfoService _userInfoService;
        private readonly dbSoundBetterContext _context;

        public ShoppingCartController(UserInfoService userInfoService, dbSoundBetterContext dbSoundBetterContext)
        {
            _userInfoService = userInfoService;
            _context = dbSoundBetterContext;
        }

        public IActionResult List()
        {
            // 取得Session
            int memberId = _userInfoService.GetMemberId();
            var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();

            cart = checkShoppingCart(cart);

            return View(cart);
        }

        /// <summary>
        /// 點選確認購買 至驗證頁
        /// </summary>
        public IActionResult Checkout()
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

        public IActionResult CheckOut_Rst()
        {
            // 






            return View();
        }


        /// <summary>
        /// 依照Session內 ProductId 檢查，賦予資料庫內值
        /// </summary>
        public List<ShoppingCartVM> checkShoppingCart(List<ShoppingCartVM> cart)
        {
            foreach (ShoppingCartVM item in cart)
            {
                // 購物車內Class資料更新，檢查待補
                var classData = _context.TClasses.FirstOrDefault(m => m.FClassId == item.ProductId);
                
                if (classData != null )
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
                    item.ProductStatus = 0000;
                }
                


            }
            return cart;
        }
    }

}
