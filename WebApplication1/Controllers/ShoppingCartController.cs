﻿using Microsoft.AspNetCore.Mvc;
using prjMusicBetter.Helpers;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using System.Formats.Asn1;

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

        /// <summary>
        /// 購物車
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 結帳處理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckOut_Rst()
        {

            // 取得Session
            //int memberId = _userInfoService.GetMemberId();
            //var cart = HttpContext.Session.Get<List<ShoppingCartVM>>($"ShoppingCart_{memberId}") ?? new List<ShoppingCartVM>();

            //cart = checkShoppingCart(cart);

            // 算總價
            var totalPrice = 0;
            //foreach (var item in cart)
            //{
            //    totalPrice += Convert.ToInt32(item.ProductPrice * item.ProductCount);
            //}
            // 儲存至資料表DealClass
            TDealClass dealClass = new TDealClass();
            dealClass.FMemberId = 2;
            dealClass.FDealdate = DateTime.Now;
            dealClass.FPrice = 6500;

            _context.Add(dealClass);
            //_context.SaveChanges();

            //儲存至資料表DealClassDetail
            //foreach (var item in cart)
            //{
                TDealClassDetail dealClassDetail = new TDealClassDetail();
                dealClassDetail.FMemberId = 2;
                dealClassDetail.FDealClassId = 4102;
                dealClassDetail.FClassId = 3;
                dealClassDetail.FStartDate = DateTime.Now;
                dealClassDetail.FEndDate = DateTime.Now;

                _context.Add(dealClassDetail);

            TDealClassDetail dealClassDetail2 = new TDealClassDetail();
            dealClassDetail2.FMemberId = 2;
            dealClassDetail2.FDealClassId = 4102;
            dealClassDetail2.FClassId = 5036;
            dealClassDetail2.FStartDate = DateTime.Now;
            dealClassDetail2.FEndDate = DateTime.Now;

            _context.Add(dealClassDetail2);
            _context.SaveChanges();
            //}


            return Json(new { success = true, message = "購買成功" });

        }


        public IActionResult CheckOut_Fin()
        {


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
        [HttpPost]
        public IActionResult ClearCart()
        {
            // 取得Session
            int memberId = _userInfoService.GetMemberId();

            // 清除购物车信息
            HttpContext.Session.Remove($"ShoppingCart_{memberId}");
            return Ok();
        }

       
        [HttpPost]
        public IActionResult ValidateCoupon(string couponCode)
        {
          

            var coupon = _context.TCoupons.SingleOrDefault(c => c.FCouponCode == couponCode);

            if (coupon != null)

            {
                return Json(new { valid = true, discount = coupon.FCouponContent });
            }
            else
            {
                return Json(new { valid = false, message = "無效的優惠券代碼" });
            }
        }

    }
}


