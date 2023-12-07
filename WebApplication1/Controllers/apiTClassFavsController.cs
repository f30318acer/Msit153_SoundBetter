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
    public class apiTClassFavsController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public apiTClassFavsController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: apiTClassFavs
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TClassFavs.Include(t => t.FClass).Include(t => t.FMember);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: apiTClassFavs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TClassFavs == null)
            {
                return NotFound();
            }

            var tClassFav = await _context.TClassFavs
                .Include(t => t.FClass)
                .Include(t => t.FMember)
                .FirstOrDefaultAsync(m => m.FClassFav == id);
            if (tClassFav == null)
            {
                return NotFound();
            }

            return View(tClassFav);
        }

        // GET: apiTClassFavs/Create
        public IActionResult Create()
        {
            ViewData["FClassId"] = new SelectList(_context.TClasses, "FClassId", "FClassId");
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId");
            return View();
        }

        // POST: apiTClassFavs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FClassFav,FClassId,FMemberId")] TClassFav tClassFav)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tClassFav);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FClassId"] = new SelectList(_context.TClasses, "FClassId", "FClassId", tClassFav.FClassId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClassFav.FMemberId);
            return View(tClassFav);
        }

        // GET: apiTClassFavs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TClassFavs == null)
            {
                return NotFound();
            }

            var tClassFav = await _context.TClassFavs.FindAsync(id);
            if (tClassFav == null)
            {
                return NotFound();
            }
            ViewData["FClassId"] = new SelectList(_context.TClasses, "FClassId", "FClassId", tClassFav.FClassId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClassFav.FMemberId);
            return View(tClassFav);
        }

        // POST: apiTClassFavs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FClassFav,FClassId,FMemberId")] TClassFav tClassFav)
        {
            if (id != tClassFav.FClassFav)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tClassFav);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TClassFavExists(tClassFav.FClassFav))
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
            ViewData["FClassId"] = new SelectList(_context.TClasses, "FClassId", "FClassId", tClassFav.FClassId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tClassFav.FMemberId);
            return View(tClassFav);
        }

        // GET: apiTClassFavs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TClassFavs == null)
            {
                return NotFound();
            }

            var tClassFav = await _context.TClassFavs
                .Include(t => t.FClass)
                .Include(t => t.FMember)
                .FirstOrDefaultAsync(m => m.FClassFav == id);
            if (tClassFav == null)
            {
                return NotFound();
            }

            return View(tClassFav);
        }

        // POST: apiTClassFavs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TClassFavs == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TClassFavs'  is null.");
            }
            var tClassFav = await _context.TClassFavs.FindAsync(id);
            if (tClassFav != null)
            {
                _context.TClassFavs.Remove(tClassFav);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TClassFavExists(int id)
        {
          return (_context.TClassFavs?.Any(e => e.FClassFav == id)).GetValueOrDefault();
        }
    }
}
