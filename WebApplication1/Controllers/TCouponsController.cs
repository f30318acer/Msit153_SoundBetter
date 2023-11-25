using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    public class TCouponsController : Controller
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;


        public TCouponsController(dbSoundBetterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: TCoupons
        //public async Task<IActionResult> Index()
        //{
        //      return _context.TCoupons != null ? 
        //                  View(await _context.TCoupons.ToListAsync()) :
        //                  Problem("Entity set 'dbSoundBetterContext.TCoupons'  is null.");
        //}
        // GET: TCoupons
        //====================================================================================
        public async Task<IActionResult> Index(string search)
        {
            ViewData["CurrentFilter"] = search;

            var coupons = from c in _context.TCoupons
                          select c;

            if (!String.IsNullOrEmpty(search))
            {
                if (int.TryParse(search, out int searchNumber))
                {
                    coupons = coupons.Where(c => c.FCouponId == searchNumber);
                }
                else
                {
                    // 可選：如果輸入不是數字，處理其他邏輯，或返回全部數據
                }
            }
            return View(await coupons.ToListAsync());
        }
//====================================================================================
        // GET: TCoupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TCoupons == null)
            {
                return NotFound();
            }

            var tCoupon = await _context.TCoupons
                .FirstOrDefaultAsync(m => m.FCouponId == id);
            if (tCoupon == null)
            {
                return NotFound();
            }

            return View(tCoupon);
        }

        // GET: TCoupons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TCoupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("FCouponId,FCouponContent,FCouponCode,FDescription,FStartdate,FEnddate,FPicture")] TCoupon tCoupon)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(tCoupon);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tCoupon);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FCouponId,FCouponContent,FCouponCode,FDescription,FStartdate,FEnddate")] TCoupon tCoupon, IFormFile FPicture)
        {
            if (ModelState.IsValid)
            {
                if (FPicture != null && FPicture.Length > 0)
                {
                    var path = Path.Combine(_environment.WebRootPath, "img", FPicture.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await FPicture.CopyToAsync(stream);
                    }

                    tCoupon.FPicture = FPicture.FileName; // 儲存檔案名
                }

                _context.Add(tCoupon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tCoupon);
        }

        // GET: TCoupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TCoupons == null)
            {
                return NotFound();
            }

            var tCoupon = await _context.TCoupons.FindAsync(id);
            if (tCoupon == null)
            {
                return NotFound();
            }
            return View(tCoupon);
        }

        // POST: TCoupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FCouponId,FCouponContent,FCouponCode,FDescription,FStartdate,FEnddate,FPicture")] TCoupon tCoupon)
        {
            if (id != tCoupon.FCouponId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tCoupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TCouponExists(tCoupon.FCouponId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tCoupon);
        }

        // GET: TCoupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TCoupons == null)
            {
                return NotFound();
            }

            var tCoupon = await _context.TCoupons
                .FirstOrDefaultAsync(m => m.FCouponId == id);
            if (tCoupon == null)
            {
                return NotFound();
            }

            return View(tCoupon);
        }

        // POST: TCoupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TCoupons == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TCoupons'  is null.");
            }
            var tCoupon = await _context.TCoupons.FindAsync(id);
            if (tCoupon != null)
            {
                _context.TCoupons.Remove(tCoupon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //==========================================================20231123新增
        [HttpGet]
        public async Task<IActionResult> GetCoupons(string search)
        {
            var couponsQuery = _context.TCoupons.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                if (int.TryParse(search, out int searchNumber))
                {
                    couponsQuery = couponsQuery.Where(c => c.FCouponId == searchNumber);
                }
                // 可選的額外處理
            }

            var coupons = await couponsQuery.ToListAsync();
            return Json(coupons);
        }
//========================================================================

        private bool TCouponExists(int id)
        {
          return (_context.TCoupons?.Any(e => e.FCouponId == id)).GetValueOrDefault();
        }
    }
}
