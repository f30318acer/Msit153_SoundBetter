using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using prjMusicBetter.Models;
using prjMusicBetter.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjMusicBetter.Controllers
{
    public class TSitesController : Controller
    {
        private readonly dbSoundBetterContext _context;

        public TSitesController(dbSoundBetterContext context)
        {
            _context = context;
        }

        // GET: TSites
        public async Task<IActionResult> Index()
        {
            var dbSoundBetterContext = _context.TSites.Include(t => t.FCity).Include(t => t.FMember);
            return View(await dbSoundBetterContext.ToListAsync());
        }

        // GET: TSites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TSites == null)
            {
                return NotFound();
            }

            var tSite = await _context.TSites
                .Include(t => t.FCity)
                .Include(t => t.FMember)
                .FirstOrDefaultAsync(m => m.FSiteId == id);

            var tSitePicture = await _context.TSitePictures
                .FirstOrDefaultAsync(f => f.FSiteId == id);

            var siteViewModel = new SiteViewModel
            {
                TSite = tSite,
                TSitePicture = tSitePicture
            };

            if (siteViewModel == null)
            {
                return NotFound();
            }

            return View(siteViewModel);
        }

        // GET: TSites/Create
        public IActionResult Create()
        {
            ViewData["FCity"] = new SelectList(_context.TCities, "FCity", "FCity");
            ViewData["FName"] = new SelectList(_context.TMembers, "FName", "FName");
            return View();
        }

        // POST: TSites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FSiteId,FSiteName,FMemberId,FPhone,FSiteType,FCityId,FAddress")] TSite tSite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tSite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FCityId"] = new SelectList(_context.TCities, "FCityId", "FCityId", tSite.FCityId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tSite.FMemberId);
            return View(tSite);
        }

        // GET: TSites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TSites == null)
            {
                return NotFound();
            }

            var tSite = await _context.TSites.FindAsync(id);
            if (tSite == null)
            {
                return NotFound();
            }
            ViewData["FCityId"] = new SelectList(_context.TCities, "FCityId", "FCityId", tSite.FCityId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tSite.FMemberId);
            return View(tSite);
        }

        // POST: TSites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FSiteId,FSiteName,FMemberId,FPhone,FSiteType,FCityId,FAddress")] TSite tSite)
        {
            if (id != tSite.FSiteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tSite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TSiteExists(tSite.FSiteId))
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
            ViewData["FCityId"] = new SelectList(_context.TCities, "FCityId", "FCityId", tSite.FCityId);
            ViewData["FMemberId"] = new SelectList(_context.TMembers, "FMemberId", "FMemberId", tSite.FMemberId);
            return View(tSite);
        }

        // GET: TSites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TSites == null)
            {
                return NotFound();
            }

            var tSite = await _context.TSites
                .Include(t => t.FCity)
                .Include(t => t.FMember)
                .FirstOrDefaultAsync(m => m.FSiteId == id);
            if (tSite == null)
            {
                return NotFound();
            }

            return View(tSite);
        }

        // POST: TSites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TSites == null)
            {
                return Problem("Entity set 'dbSoundBetterContext.TSites'  is null.");
            }
            var tSite = await _context.TSites.FindAsync(id);
            if (tSite != null)
            {
                _context.TSites.Remove(tSite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TSiteExists(int id)
        {
            return (_context.TSites?.Any(e => e.FSiteId == id)).GetValueOrDefault();
        }
    }
}
