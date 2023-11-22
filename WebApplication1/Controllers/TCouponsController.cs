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

        public TCouponsController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TCoupons
        public async Task<IActionResult> Index()
        {
              return _context.TCoupons != null ? 
                          View(await _context.TCoupons.ToListAsync()) :
                          Problem("Entity set 'dbSoundBetterContext.TCoupons'  is null.");
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FCouponId,FCouponContent,FCouponCode,FDescription,FStartdate,FEnddate,FPicture")] TCoupon tCoupon)
        {
            if (ModelState.IsValid)
            {
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

        private bool TCouponExists(int id)
        {
          return (_context.TCoupons?.Any(e => e.FCouponId == id)).GetValueOrDefault();
        }
    }
}
