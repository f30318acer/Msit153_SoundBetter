using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiTCouponsController : ControllerBase
    {
        private readonly dbSoundBetterContext _context;

        public apiTCouponsController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: api/apiTCoupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TCoupon>>> GetTCoupons()
        {
          if (_context.TCoupons == null)
          {
              return NotFound();
          }
            return await _context.TCoupons.ToListAsync();
        }

        // GET: api/apiTCoupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TCoupon>> GetTCoupon(int id)
        {
          if (_context.TCoupons == null)
          {
              return NotFound();
          }
            var tCoupon = await _context.TCoupons.FindAsync(id);

            if (tCoupon == null)
            {
                return NotFound();
            }

            return tCoupon;
        }

        // PUT: api/apiTCoupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTCoupon(int id, TCoupon tCoupon)
        {
            if (id != tCoupon.FCouponId)
            {
                return BadRequest();
            }

            _context.Entry(tCoupon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TCouponExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/apiTCoupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TCoupon>> PostTCoupon(TCoupon tCoupon)
        {
          if (_context.TCoupons == null)
          {
              return Problem("Entity set 'dbSoundBetterContext.TCoupons'  is null.");
          }
            _context.TCoupons.Add(tCoupon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTCoupon", new { id = tCoupon.FCouponId }, tCoupon);
        }

        // DELETE: api/apiTCoupons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTCoupon(int id)
        {
            if (_context.TCoupons == null)
            {
                return NotFound();
            }
            var tCoupon = await _context.TCoupons.FindAsync(id);
            if (tCoupon == null)
            {
                return NotFound();
            }

            _context.TCoupons.Remove(tCoupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TCouponExists(int id)
        {
            return (_context.TCoupons?.Any(e => e.FCouponId == id)).GetValueOrDefault();
        }
    }
}
